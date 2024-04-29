using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Components;

public class ComponentRequest
{
    public ComponentType Type { get; init; }
    public DateTime Received { get; init; }
    public double Amount { get; set; }
}