using DelTSZ.Models.Enums;
using DelTSZ.Models.Users;

namespace DelTSZ.Models.Products;

public interface IProduct
{
    //Properties
    public int Id { get; init; }

    //Navigation properties
    public string? UserId { get; init; }
    public User? User { get; init; }
}