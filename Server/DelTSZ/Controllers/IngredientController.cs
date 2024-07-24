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
            return BadRequest(new { message = "Error getting ingredient types." });
        }
    }

    [HttpGet("owner"), Authorize(Roles = "Customer")]
    public async Task<ActionResult<IEnumerable<IngredientSumResponse>>> GetAllOwnerIngredients()
    {
        try
        {
            return Ok(await ingredientRepository.GetAllOwnerIngredientsSumByType());
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error getting ingredients." });
        }
    }

    [HttpGet("producers"), Authorize(Roles = "Owner")]
    public async Task<ActionResult<IEnumerable<IngredientResponse>>> GetAllProducerIngredients()
    {
        try
        {
            return Ok(await ingredientRepository.GetAllProducerIngredientAmountsByType());
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error getting ingredients." });
        }
    }

    [HttpPost, Authorize(Roles = "Producer")]
    public async Task<IActionResult> ProvideIngredient([Required] IngredientRequest ingredientRequest,
        int days)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;

            if (id == null || !Enum.IsDefined(typeof(IngredientType), ingredientRequest.Type))
            {
                return Conflict(new { message = "Wrong user id or ingredient type." });
            }

            await ingredientRepository.CreateOrUpdateIngredient(ingredientRequest, id, days);

            return Ok(new { message = "Ingredient provide successful." });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error create ingredient." });
        }
    }

    [HttpPut("{id:int}"), Authorize(Roles = "Owner")]
    public async Task<IActionResult> ReceiveIngredientById(int id)
    {
        try
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;

            if (userId == null)
            {
                return Conflict(new { message = "Wrong user id." });
            }

            var ingredient = await ingredientRepository.GetIngredientById(id);

            if (ingredient == null)
            {
                return NotFound(new { message = "Ingredient not found." });
            }

            await ingredientRepository.IngredientUpdateById(id, userId);
            return Ok(new { message = "Ingredient receive successful." });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error getting ingredient." });
        }
    }

    [HttpPut("{type}/{amount:decimal}"), Authorize(Roles = "Customer")]
    public async Task<IActionResult> UpdateIngredientByType(IngredientType type, decimal amount)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;

            if (id == null || !Enum.IsDefined(typeof(IngredientType), type) || amount < 0)
            {
                return Conflict(new { message = "Wrong user id, ingredient type or amount." });
            }

            var ownerIngredientAmount = await ingredientRepository.GetAllOwnerIngredientAmountsByType(type);

            if (ownerIngredientAmount < amount)
            {
                return Conflict(new { message = "Not enough ingredients." });
            }

            await ingredientRepository.IngredientUpdateByRequestAmount(type, id, amount);
            return Ok(new { message = "Ingredient order successful." });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error update ingredient." });
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
                return NotFound(new { message = "Ingredient not found." });
            }

            await ingredientRepository.DeleteIngredient(ingredient);

            return Ok(new { message = "Ingredient delete successful." });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error getting ingredient." });
        }
    }
}