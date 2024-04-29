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
            return NotFound("Error getting products.");
        }
    }

    [HttpPost, Authorize(Roles = "Producer")]
    public ActionResult<ComponentRequest> CreateComponent([Required] ComponentRequest component)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))!.Value;

            if (id == null || !Enum.IsDefined(typeof(ComponentType), component.Type))
            {
                return Conflict("Wrong user id or product type.");
            }

            componentRepository.AddComponentToUser(component, id);
            return Ok(component);
        }
        catch (Exception)
        {
            return BadRequest("Error create product.");
        }
    }

    [HttpPut("{type}"), Authorize(Roles = "Owner, Costumer")]
    public async Task<IActionResult> UpdateComponentByType(ComponentType type, double amount)
    {
        try
        {
            var component = await componentRepository.GetOldestComponent(type);
            if (component == null)
            {
                return NotFound("Product not found.");
            }

            if (component.Amount - amount <= 0)
            {
                componentRepository.DeleteComponent(component);
                return Conflict(new { message = "Leftover: ", leftover = +component.Amount - amount });
            }

            component.Amount -= amount;

            componentRepository.UpdateComponent(component);
            return Ok("Product update successful.");
        }
        catch (Exception)
        {
            return BadRequest("Error update product.");
        }
    }

    [HttpDelete("{id}"), Authorize(Roles = "Owner, Producer")]
    public async Task<ActionResult<ComponentResponse>> DeleteComponentById(int id)
    {
        try
        {
            var component = await componentRepository.GetComponentById(id);
            componentRepository.DeleteComponent(component!);

            if (component == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(new ComponentResponse
            {
                Id = component.Id,
                Amount = component.Amount,
                Type = component.Type,
                Received = component.Received,
                UserId = component.UserId
            });
        }
        catch (Exception)
        {
            return NotFound("Error getting product.");
        }
    }
}