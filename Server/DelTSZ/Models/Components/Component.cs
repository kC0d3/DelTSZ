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
    public double Amount { get; set; }

    //Navigation properties

    public string? UserId { get; init; }
    public User? User { get; init; }
    public int? ProductId { get; init; }
    public Product? Product { get; init; }
}