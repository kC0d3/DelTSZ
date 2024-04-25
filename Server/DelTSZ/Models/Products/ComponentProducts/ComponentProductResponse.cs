using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Products.ComponentProducts;

public class ComponentProductResponse : IProductResponse
{
    public int Id { get; init; }
    public ComponentProductType ProductType { get; init; }
    public DateTime Received { get; init; }
    public double Amount { get; init; }
    public string? UserId { get; init; }
}