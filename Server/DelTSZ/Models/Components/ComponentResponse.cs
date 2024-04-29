using DelTSZ.Models.Enums;
using DelTSZ.Models.Products;

namespace DelTSZ.Models.Components;

public class ComponentResponse
{
    public int Id { get; init; }
    public ComponentType Type { get; init; }
    public DateTime Received { get; init; }
    public double Amount { get; init; }
    public string? UserId { get; init; }
}