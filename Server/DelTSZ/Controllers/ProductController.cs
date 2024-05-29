using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Ingredients;
using DelTSZ.Models.Products;
using DelTSZ.Repositories.IngredientRepository;
using DelTSZ.Repositories.ProductRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IProductRepository productRepository, IIngredientRepository ingredientRepository)
    : ControllerBase
{
    [HttpGet("types")]
    public ActionResult<IEnumerable<EnumResponse>> GetProductTypes()
    {
        try
        {
            return Ok(ProductTypeExtensions.GetProductTypes());
        }
        catch (Exception)
        {
            return NotFound(new { message = "Error getting product types." });
        }
    }

    [HttpGet("sum"), Authorize(Roles = "Costumer")]
    public async Task<ActionResult<IEnumerable<IngredientRequest>>> GetAllOwnerProducts()
    {
        try
        {
            return Ok(await productRepository.GetAllOwnerProductsSumByType());
        }
        catch (Exception)
        {
            return NotFound(new { message = "Error getting products." });
        }
    }

    [HttpPost, Authorize(Roles = "Owner")]
    public async Task<IActionResult> CreateProduct([Required] ProductRequest productRequest, int days)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;

            if (id == null || !Enum.IsDefined(typeof(ProductType), productRequest.Type))
            {
                return Conflict(new { message = "Wrong user id or product type." });
            }

            var productDetails = productRequest.Type.GetProductDetails().ToList();

            foreach (var pd in productDetails)
            {
                var demandAmount = pd.Item1 * productRequest.Amount;
                var ownerIngredientsAmount = await ingredientRepository.GetAllOwnerIngredientAmountsByType(pd.Item2);

                if (ownerIngredientsAmount < demandAmount)
                {
                    return Conflict(new { message = "Not enough ingredients." });
                }
            }

            await productRepository.CreateOrUpdateProduct(productRequest, id, days);

            return Ok(new { message = "Product create successful." });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error create product." });
        }
    }

    [HttpPut("{type}/{amount:int}"), Authorize(Roles = "Costumer")]
    public async Task<IActionResult> UpdateProductByType(ProductType type, int amount)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;

            if (id == null || !Enum.IsDefined(typeof(ProductType), type) || amount < 0)
            {
                return Conflict(new { message = "Wrong user id, product type or amount." });
            }

            var ownerProductAmount = await productRepository.GetAllOwnerProductsAmountsByType(type);

            if (ownerProductAmount < amount)
            {
                return Conflict(new { message = "Not enough products." });
            }

            await productRepository.ProductUpdateByRequestAmount(type, id, amount);
            return Ok(new { message = "Product update successful." });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error update component." });
        }
    }

    [HttpDelete("{id:int}"), Authorize(Roles = "Owner")]
    public async Task<IActionResult> DeleteProductById(int id)
    {
        try
        {
            var product = await productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            await productRepository.DeleteProduct(product);
            return Ok(new { message = "Product delete successful." });
        }
        catch (Exception)
        {
            return NotFound(new { message = "Error getting product." });
        }
    }
}