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

    [HttpPost, Authorize] //Authorize Roles should be Grower
    public ActionResult<ComponentRequest> CreateComponent([Required] ComponentRequest product)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))!.Value;

            if (id == null || !Enum.IsDefined(typeof(ComponentType), product.Type))
            {
                return Conflict("Wrong user id or product type.");
            }

            componentRepository.AddComponentToUser(product, id);
            return Ok(product);
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
            var product = await componentRepository.GetOldestComponent(type);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            if (product.Amount - amount <= 0)
            {
                componentRepository.DeleteComponent(product);
                return Conflict(new { message = "Leftover: ", leftover = +product.Amount - amount });
            }

            product.Amount -= amount;

            componentRepository.UpdateComponent(product);
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
            var product = await componentRepository.GetComponentById(id);
            componentRepository.DeleteComponent(product!);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(new ComponentResponse
            {
                Id = product.Id,
                Amount = product.Amount,
                Type = product.Type,
                Received = product.Received,
                UserId = product.UserId
            });
        }
        catch (Exception)
        {
            return NotFound("Error getting product.");
        }
    }
}