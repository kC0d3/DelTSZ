using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using DelTSZ.Models.Addresses;
using DelTSZ.Models.Ingredients;
using DelTSZ.Models.Products;

namespace DelTSZ.Models.Users;

public class User : IdentityUser
{
    //Properties
    [StringLength(100)] public string? CompanyName { get; init; }
    [StringLength(50)] public string? Role { get; init; }

    //Navigation Properties
    public Address? Address { get; init; }
    public IEnumerable<Ingredient>? Ingredients { get; init; }
    public IEnumerable<Product>? Products { get; init; }
}