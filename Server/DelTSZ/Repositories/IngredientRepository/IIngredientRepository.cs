using DelTSZ.Models.Enums;
using DelTSZ.Models.Ingredients;

namespace DelTSZ.Repositories.IngredientRepository;

public interface IIngredientRepository
{
    Task<IEnumerable<IngredientSumResponse>> GetAllOwnerIngredientsSumByType();
    Task<decimal> GetAllOwnerIngredientAmountsByType(IngredientType type);
    Task<IEnumerable<IngredientResponse>> GetAllProducerIngredientAmountsByType();
    Task<Ingredient?> GetOwnerOldestIngredientByType(IngredientType type);
    Task<Ingredient?> GetIngredientById(int id);
    Task IngredientUpdateByRequestAmount(IngredientType type, string id, decimal amount);
    Task CreateOrUpdateIngredient(IngredientRequest ingredientRequest, string id, int days);
    Task IngredientUpdateById(int id, string userId);
    Task UpdateIngredient(Ingredient ingredient);
    Task DeleteIngredient(Ingredient ingredient);
}