using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Ingredients;
using DelTSZ.Repositories.IngredientRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/ingredients")]
[ApiController]
public class IngredientController(IIngredientRepository ingredientRepository) : ControllerBase
{
    [HttpGet("types")]
    public ActionResult<IEnumerable<EnumResponse>> GetIngredientTypes()
    {
        try
        {
            return Ok(IngredientTypeExtensions.GetIngredientTypes());
        }
        catch (Exception)
        {
            return NotFound("Error getting ingredient types.");
        }
    }

    [HttpGet("sum"), Authorize(Roles = "Costumer")]
    public async Task<ActionResult<IEnumerable<IngredientSumResponse>>> GetAllOwnerIngredients()
    {
        try
        {
            return Ok(await ingredientRepository.GetAllOwnerIngredientsSumByType());
        }
        catch (Exception)
        {
            return NotFound("Error getting ingredients.");
        }
    }

    [HttpPost, Authorize(Roles = "Producer")]
    public async Task<IActionResult> CreateIngredient([Required] IngredientRequest ingredientRequest,
        int days)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;

            if (id == null || !Enum.IsDefined(typeof(IngredientType), ingredientRequest.Type))
            {
                return Conflict("Wrong user id or ingredient type.");
            }

            var ingredient =
                await ingredientRepository.GetIngredientByUserId_Type_ReceivedDate(ingredientRequest.Type, id, days);

            if (ingredient == null)
            {
                await ingredientRepository.CreateIngredientToUser(ingredientRequest, id, days);
            }
            else
            {
                ingredient.Amount += ingredientRequest.Amount;
                await ingredientRepository.UpdateIngredient(ingredient);
            }

            return Ok("Ingredient create successful.");
        }
        catch (Exception)
        {
            return BadRequest("Error create ingredient.");
        }
    }

    [HttpPut("{id:int}"), Authorize(Roles = "Owner")]
    public async Task<IActionResult> UpdateIngredientById(int id)
    {
        try
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;

            if (userId == null)
            {
                return Conflict("Wrong user id.");
            }

            var ingredient = await ingredientRepository.GetIngredientById(id);

            if (ingredient == null)
            {
                return NotFound("Ingredient not found.");
            }

            await ingredientRepository.IngredientUpdateById(id, userId);
            return Ok("Ingredient update successful.");
        }
        catch (Exception)
        {
            return NotFound("Error getting ingredient.");
        }
    }

    [HttpPut("{type}"), Authorize(Roles = "Costumer")]
    public async Task<IActionResult> UpdateIngredientByType(IngredientType type, decimal amount)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;

            if (id == null || !Enum.IsDefined(typeof(IngredientType), type) || amount < 0)
            {
                return Conflict("Wrong user id, ingredient type or amount.");
            }

            var ownerIngredientAmount = await ingredientRepository.GetAllOwnerIngredientAmountsByType(type);

            if (ownerIngredientAmount < amount)
            {
                return Conflict(new { message = "Not enough ingredients." });
            }

            await ingredientRepository.IngredientUpdateByRequestAmount(type, id, amount);
            return Ok("Ingredient update successful.");
        }
        catch (Exception)
        {
            return BadRequest("Error update ingredient.");
        }
    }

    [HttpDelete("{id:int}"), Authorize(Roles = "Owner")]
    public async Task<IActionResult> DeleteIngredientById(int id)
    {
        try
        {
            var ingredient = await ingredientRepository.GetIngredientById(id);

            if (ingredient == null)
            {
                return NotFound("Ingredient not found.");
            }

            await ingredientRepository.DeleteIngredient(ingredient);
            return Ok("Ingredient delete successful.");
        }
        catch (Exception)
        {
            return NotFound("Error getting ingredient.");
        }
    }
}