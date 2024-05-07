using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Components;

public class ComponentUpdateRequest
{
    public ComponentType Type { get; init; }
    public DateTime Received { get; init; }
    public decimal Amount { get; init; }
}