using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Products.ComponentProducts;
using DelTSZ.Repositories.ComponentProductRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/component-products")]
[ApiController]
public class ComponentProductController(IComponentProductRepository componentProductRepository) : ControllerBase
{
    [HttpGet, Authorize(Roles = "Costumer")]
    public async Task<ActionResult<IEnumerable<ComponentProductRequest>>> GetAllOwnerSingleProducts()
    {
        try
        {
            return Ok(await componentProductRepository.GetAllOwnerComponentProducts());
        }
        catch (Exception)
        {
            return NotFound("Error getting products.");
        }
    }
    
    [HttpPost, Authorize]
    public ActionResult<ComponentProductRequest> CreateComponentProduct([Required] ComponentProductRequest product)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))!.Value;

            if (id == null || !Enum.IsDefined(typeof(ComponentProductType), product.ProductType))
            {
                return Conflict("Wrong user id or product type.");
            }

            componentProductRepository.AddComponentProductToUser(product, id);
            return Ok(product);
        }
        catch (Exception)
        {
            return BadRequest("Error create product.");
        }
    }

}