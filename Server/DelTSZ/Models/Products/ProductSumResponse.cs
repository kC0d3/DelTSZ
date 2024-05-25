using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Products;

public class ProductSumResponse
{
    public ProductType Type { get; init; }
    public int Amount { get; init; }
}