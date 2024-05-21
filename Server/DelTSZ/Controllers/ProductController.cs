﻿using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Ingredients;
using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;
using DelTSZ.Repositories.IngredientRepository;
using DelTSZ.Repositories.ProductIngredientRepository;
using DelTSZ.Repositories.ProductRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(
    IProductRepository productRepository,
    IIngredientRepository ingredientRepository,
    IProductIngredientRepository productIngredientRepository)
    : ControllerBase
{
    [HttpGet, Authorize(Roles = "Costumer")]
    public async Task<ActionResult<IEnumerable<IngredientRequest>>> GetAllOwnerProducts()
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
    public async Task<IActionResult> CreateProduct([Required] ProductRequest productRequest, int days)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;

            if (id == null || !Enum.IsDefined(typeof(ProductType), productRequest.Type))
            {
                return Conflict("Wrong user id or product type.");
            }

            var productDetails = productRequest.Type.GetProductDetails().ToList();

            foreach (var pd in productDetails)
            {
                var demandAmount = pd.Item1 * productRequest.Amount;
                var ownerIngredientsAmount = await ingredientRepository.GetAllOwnerIngredientAmountsByType(pd.Item2);

                if (ownerIngredientsAmount < demandAmount)
                {
                    return Conflict("Not enough ingredients.");
                }
            }

            var product =
                await productRepository.GetProductByUserId_Type_PackedDate(productRequest.Type, id, days);
            var ingredients = new List<ProductIngredient>();

            if (product == null)
            {
                ingredients.AddRange(await productIngredientRepository.CreateProductIngredients(productRequest));
                await productRepository.CreateProductToUser(productRequest, id, ingredients, days);
            }
            else
            {
                product.Amount += productRequest.Amount;
                await productIngredientRepository.IncreaseProductIngredientsFromOwnerIngredients(product, productRequest.Amount);
                await productRepository.UpdateProduct(product);
            }

            return Ok("Product create successful.");
        }
        catch (Exception)
        {
            return BadRequest("Error create product.");
        }
    }
}