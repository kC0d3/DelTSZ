using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Ingredients;

public class IngredientSumResponse
{
    public IngredientType Type { get; init; }
    public decimal Amount { get; init; }
}