using DelTSZ.Models.Enums;
using DelTSZ.Models.Ingredients;

namespace DelTSZ.Repositories.IngredientRepository;

public interface IIngredientRepository
{
    Task<IEnumerable<IngredientSumResponse>> GetAllOwnerIngredientsSumByType();
    Task<decimal> GetAllOwnerIngredientAmountsByType(IngredientType type);
    Task<Ingredient?> GetOwnerOldestIngredientByType(IngredientType type);
    Task<Ingredient?> GetIngredientById(int id);
    Task IngredientUpdateByRequestAmount(IngredientType type, string id, decimal amount);
    Task IngredientUpdateById(int id, string userId);
    Task UpdateIngredient(Ingredient ingredient);
    Task DeleteIngredient(Ingredient ingredient);
}