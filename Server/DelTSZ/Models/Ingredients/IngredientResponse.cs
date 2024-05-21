using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Ingredients;

public class IngredientResponse
{
    public int Id { get; init; }
    public IngredientType Type { get; init; }
    public DateTime Received { get; init; }
    public decimal Amount { get; init; }
}