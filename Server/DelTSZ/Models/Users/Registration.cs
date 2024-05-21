using DelTSZ.Models.Addresses;

namespace DelTSZ.Models.Users;

public record Registration(
    string Email,
    string Username,
    string Companyname,
    AddressRequest Address,
    string Password
);