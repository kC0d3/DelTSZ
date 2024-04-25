using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Products.ComponentProducts;

public class ComponentProductRequest
{
    public ComponentProductType ProductType { get; init; }
    public DateTime Received { get; init; }
    public double Amount { get; set; }
}