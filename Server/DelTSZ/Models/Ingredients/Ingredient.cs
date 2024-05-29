using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Users;

namespace DelTSZ.Models.Ingredients;

public class Ingredient
{
    //Properties

    public int Id { get; init; }
    public IngredientType Type { get; init; }
    public DateTime Received { get; init; }
    public decimal Amount { get; set; }

    //Navigation properties

    [StringLength(450)] public string? UserId { get; set; }
    public User? User { get; init; }
}