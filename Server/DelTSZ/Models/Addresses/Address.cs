using DelTSZ.Models.Users;

namespace DelTSZ.Models.Addresses;
public class Address
{
    //Properties
    public int Id { get; init; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }

    //Navigation Properties
    public string? UserId { get; init; }
    public User? User { get; init; }
}