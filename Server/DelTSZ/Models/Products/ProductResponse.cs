using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Products;

public class ProductResponse
{
    public int Id { get; init; }
    public ProductType Type { get; init; }
    public DateTime Packed { get; init; }
    public int Amount { get; init; }
}