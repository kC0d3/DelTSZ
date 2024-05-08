using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Components;

public class ComponentSumResponse
{
    public ComponentType Type { get; init; }
    public decimal Amount { get; init; }
}