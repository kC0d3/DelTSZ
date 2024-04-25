using DelTSZ.Models.Enums;
using DelTSZ.Models.Users;

namespace DelTSZ.Models.Products.ComponentProducts;

public class ComponentProduct : IProduct
{
    //Properties
    
    public int Id { get; init; }
    public ComponentProductType ProductType { get; init; }
    public DateTime Received { get; init; }
    public double Amount { get; set; }
    
    //Navigation properties
    
    public string? UserId { get; init; }
    public User? User { get; init; }
}