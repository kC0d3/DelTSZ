using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Ingredients;

public class IngredientRequest
{
    public IngredientType Type { get; set; }
    public decimal Amount { get; set; }
}