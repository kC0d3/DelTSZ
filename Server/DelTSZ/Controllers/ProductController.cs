using DelTSZ.Models.Components;
using DelTSZ.Repositories.ProductRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IProductRepository productRepository) : ControllerBase
{
    [HttpGet, Authorize(Roles = "Costumer")]
    public async Task<ActionResult<IEnumerable<ComponentRequest>>> GetAllOwnerProducts()
    {
        try
        {
            return Ok(await productRepository.GetAllOwnerProducts());
        }
        catch (Exception)
        {
            return NotFound("Error getting products.");
        }
    }
}