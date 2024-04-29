using DelTSZ.Models.Components;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Users;

namespace DelTSZ.Models.Products;

public class Product
{
    //Properties
    public int Id { get; init; }
    public ProductType Type { get; init; }
    public DateTime Packed { get; init; }
    public int Amount { get; set; }
    public ICollection<Component>? Components { get; init; }

    //Navigation properties
    public string? UserId { get; init; }
    public User? User { get; init; }
}