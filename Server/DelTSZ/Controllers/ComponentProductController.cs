using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Products;
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
    public async Task<ActionResult<IEnumerable<ComponentProductRequest>>> GetAllOwnerComponentProducts()
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

    [HttpPut("{type}"), Authorize(Roles = "Owner, Costumer")]
    public async Task<IActionResult> UpdateComponentProductByType(ComponentProductType type, double amount)
    {
        try
        {
            var product = await componentProductRepository.GetOldestComponentProduct(type);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            if (product.Amount - amount <= 0)
            {
                componentProductRepository.DeleteComponentProduct(product);
                return Conflict(new { message = "Leftover: ", leftover = +product.Amount - amount });
            }

            product.Amount -= amount;

            componentProductRepository.UpdateComponentProduct(product);
            return Ok("Product update successful.");
        }
        catch (Exception)
        {
            return BadRequest("Error update product.");
        }
    }
    
    [HttpDelete("{id}"), Authorize(Roles = "Owner, Grower")]
    public async Task<ActionResult<IProductResponse>> DeleteComponentProductById(int id)
    {
        try
        {
            var product = await componentProductRepository.GetComponentProductById(id);
            componentProductRepository.DeleteComponentProduct(product!);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(new ComponentProductResponse
            {
                Id = product.Id,
                Amount = product.Amount,
                ProductType = product.ProductType,
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