namespace DelTSZ.Models.Addresses;

public class AddressRequest
{
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }
}