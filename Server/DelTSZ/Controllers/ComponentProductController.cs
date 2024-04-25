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
}