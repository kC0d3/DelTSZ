using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Components;
using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductComponents;
using DelTSZ.Models.Products;
using DelTSZ.Repositories.ComponentRepository;
using DelTSZ.Repositories.ProductRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IProductRepository productRepository, IComponentRepository componentRepository)
    : ControllerBase
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

    [HttpPost, Authorize(Roles = "Owner")]
    public async Task<IActionResult> CreateProduct([Required] ProductRequest product)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;

            if (id == null || !Enum.IsDefined(typeof(ProductType), product.Type))
            {
                return Conflict("Wrong user id or product type.");
            }

            var productDetails = product.Type.GetProductDetails().ToList();
            var components = new List<ProductComponent>();

            foreach (var pd in productDetails)
            {
                var demandAmount = pd.Item1 * product.Amount;
                var ownerComponentsAmount = await componentRepository.GetAllOwnerComponentAmountsByType(pd.Item2);

                if (ownerComponentsAmount < demandAmount)
                {
                    return Conflict("Not enough components.");
                }
            }

            foreach (var pd in productDetails)
            {
                var demandAmount = pd.Item1 * product.Amount;
                components.AddRange(await productRepository.CreateProductComponents(pd.Item2, pd.Item1, demandAmount));
            }

            productRepository.CreateProductToUser(product, id, components);
            return Ok("Product create successful.");
        }
        catch (Exception)
        {
            return BadRequest("Error create product.");
        }
    }
}