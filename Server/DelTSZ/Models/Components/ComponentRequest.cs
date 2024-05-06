using DelTSZ.Models.Enums;

namespace DelTSZ.Models.Components;

public class ComponentRequest
{
    public ComponentType Type { get; set; }
    public decimal Amount { get; set; }
}