using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Users;

namespace DelTSZ.Models.Products;

public class Product
{
    //Properties
    
    public int Id { get; init; }
    public ProductType Type { get; init; }
    public DateTime Packed { get; init; }
    public int Amount { get; set; }
    public ICollection<ProductIngredient>? Ingredients { get; set; }

    //Navigation properties
    
    [StringLength(450)] public string? UserId { get; init; }
    public User? User { get; init; }
}