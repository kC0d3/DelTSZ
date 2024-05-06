using DelTSZ.Models.Enums;
using DelTSZ.Models.Products;
using DelTSZ.Models.Users;

namespace DelTSZ.Models.Components;

public class Component
{
    //Properties

    public int Id { get; init; }
    public ComponentType Type { get; init; }
    public DateTime Received { get; init; }
    public decimal Amount { get; set; }

    //Navigation properties

    public string? UserId { get; set; }
    public User? User { get; init; }
}