using DelTSZ.Models.Addresses;
using DelTSZ.Models.Ingredients;
using DelTSZ.Models.Products;

namespace DelTSZ.Models.Users;

public class UserResponse
{
    public string? Email { get; init; }
    public string? Username { get; init; }
    public string? Companyname { get; init; }
    public string? Role { get; init; }
    public AddressResponse? Address { get; init; }
    public IEnumerable<IngredientResponse>? Ingredients { get; init; }
    public IEnumerable<ProductResponse>? Products { get; init; }
}