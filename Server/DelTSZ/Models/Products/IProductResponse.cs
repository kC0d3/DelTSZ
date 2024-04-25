using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Products;

public interface IProductResponse
{
    public int Id { get; init; }
    public string? UserId { get; init; }
}