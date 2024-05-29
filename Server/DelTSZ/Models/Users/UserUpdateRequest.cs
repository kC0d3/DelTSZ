using DelTSZ.Models.Addresses;

namespace DelTSZ.Models.Users;

public class UserUpdateRequest
{
    public string? Email { get; init; }
    public string? UserName { get; init; }
    public string? CompanyName { get; init; }
    public AddressRequest? Address { get; init; }
}