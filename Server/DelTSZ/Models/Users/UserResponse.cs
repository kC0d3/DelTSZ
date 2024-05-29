using DelTSZ.Models.Addresses;
using DelTSZ.Models.Ingredients;
using DelTSZ.Models.Products;

namespace DelTSZ.Models.Users;

public class UserResponse
{
    public string? Email { get; init; }
    public string? UserName { get; init; }
    public string? CompanyName { get; init; }
    public string? Role { get; init; }
    public AddressResponse? Address { get; init; }
    public IEnumerable<IngredientResponse>? Ingredients { get; init; }
    public IEnumerable<ProductResponse>? Products { get; init; }
}