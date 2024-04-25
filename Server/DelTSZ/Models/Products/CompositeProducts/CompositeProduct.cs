using DelTSZ.Models.Enums;
using DelTSZ.Models.Products.ComponentProducts;
using DelTSZ.Models.Users;

namespace DelTSZ.Models.Products.CompositeProducts;

public class CompositeProduct : IProduct
{
    //Properties
    public int Id { get; init; }
    public CompositeProductType ProductType { get; init; }
    public DateTime Packed { get; init; }
    public int Amount { get; set; }
    public ICollection<ComponentProduct>? Components { get; set; }

    //Navigation properties
    public string? UserId { get; init; }
    public User? User { get; init; }
}