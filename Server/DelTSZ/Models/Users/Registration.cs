using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Addresses;

namespace DelTSZ.Models.Users;

public class Registration
{
    public string? Email { get; init; }
    public string? UserName { get; init; }
    public string? CompanyName { get; init; }
    public AddressRequest? Address { get; init; }
    public string? Password { get; init; }
    
    [Compare("Password", ErrorMessage = "Confirm password does not match.")]
    public string? ConfirmPassword { get; init; }
}