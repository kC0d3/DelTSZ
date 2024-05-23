using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Users;

namespace DelTSZ.Models.Addresses;

public class Address
{
    //Properties
    public int Id { get; init; }
    [StringLength(10)] public string? ZipCode { get; set; }
    [StringLength(50)] public string? City { get; set; }
    [StringLength(100)] public string? Street { get; set; }
    [StringLength(10)] public string? HouseNumber { get; set; }

    //Navigation Properties
    [StringLength(100)] public string? UserId { get; init; }
    public User? User { get; init; }
}