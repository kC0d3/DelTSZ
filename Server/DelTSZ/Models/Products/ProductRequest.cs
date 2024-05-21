using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Products;

public class ProductRequest
{
    public ProductType Type { get; init; }
    public int Amount { get; set; }
}