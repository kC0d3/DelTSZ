using Microsoft.AspNetCore.Identity;
using DelTSZ.Models.Addresses;
using DelTSZ.Models.Products;

namespace DelTSZ.Models.Users;

public class User : IdentityUser
{
    //Properties
    public string? CompanyName { get; init; }
    public string? Role { get; init; }

    //Navigation Properties
    public Address? Address { get; init; }
    public ICollection<Product>? Products { get; init; }
}