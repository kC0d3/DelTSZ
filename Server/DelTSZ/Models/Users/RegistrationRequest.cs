using DelTSZ.Models.Addresses;

namespace DelTSZ.Models.Users;

public record RegistrationRequest(
    string Email,
    string Username,
    string Companyname,
    AddressRequest Address,
    string Password
);