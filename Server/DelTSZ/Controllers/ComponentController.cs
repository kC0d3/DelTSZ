using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Components;
using DelTSZ.Models.Enums;
using DelTSZ.Repositories.ComponentRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/components")]
[ApiController]
public class ComponentController(IComponentRepository componentRepository) : ControllerBase
{
    [HttpGet, Authorize(Roles = "Costumer")]
    public async Task<ActionResult<IEnumerable<ComponentRequest>>> GetAllOwnerComponents()
    {
        try
        {
            return Ok(await componentRepository.GetAllOwnerComponents());
        }
        catch (Exception)
        {
            return NotFound("Error getting components.");
        }
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<ComponentRequest>> CreateComponent([Required] ComponentRequest componentRequest,
        int days)
    {
        try
        {
            var id = HttpContext.User.Claims?.FirstOrDefault(c => c.Type.Contains("identifier"))!.Value;

            if (id == null || !Enum.IsDefined(typeof(ComponentType), componentRequest.Type))
            {
                return Conflict("Wrong user id or component type.");
            }

            var component = await componentRepository.GetComponentByUserIdTypeReceivedDate(componentRequest.Type, id, days);

            if (component == null)
            {
                componentRepository.CreateComponentToUser(componentRequest, id, days);
            }
            else
            {
                component.Amount += componentRequest.Amount;
                componentRepository.UpdateComponent(component);
            }

            return Ok(componentRequest);
        }
        catch (Exception)
        {
            return BadRequest("Error create component.");
        }
    }

    [HttpPut("{type}"), Authorize(Roles = "Owner, Costumer")]
    public async Task<IActionResult> UpdateComponentByType(ComponentType type, decimal amount)
    {
        try
        {
            var id = HttpContext.User.Claims?.FirstOrDefault(c => c.Type.Contains("identifier"))!.Value;

            if (id == null || !Enum.IsDefined(typeof(ComponentType), type) || amount < 0)
            {
                return Conflict("Wrong user id, component type or amount.");
            }

            var ownerComponentAmount = await componentRepository.GetAllOwnerComponentAmountsByType(type);
            
            if (ownerComponentAmount < amount)
            {
                return Conflict(new { message = "Not enough components." });
            }

            componentRepository.ComponentUpdateByRequestAmount(type, id, amount);
            return Ok("Component update successful.");
        }
        catch (Exception)
        {
            return BadRequest("Error update component.");
        }
    }

    [HttpDelete("{id}"), Authorize(Roles = "Owner, Producer")]
    public async Task<ActionResult<ComponentResponse>> DeleteComponentById(int id)
    {
        try
        {
            var component = await componentRepository.GetComponentById(id);

            if (component == null)
            {
                return NotFound("Component not found.");
            }

            componentRepository.DeleteComponent(component!);
            return Ok("Component delete successful.");
        }
        catch (Exception)
        {
            return NotFound("Error getting component.");
        }
    }
}