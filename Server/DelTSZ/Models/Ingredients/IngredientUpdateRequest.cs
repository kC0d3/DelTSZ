using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Ingredients;

public class IngredientUpdateRequest
{
    public IngredientType Type { get; init; }
    public DateTime Received { get; init; }
    public decimal Amount { get; init; }
}