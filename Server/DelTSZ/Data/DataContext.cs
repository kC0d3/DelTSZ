using DelTSZ.Models.Addresses;
using DelTSZ.Models.Products;
using DelTSZ.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Data;

public class DataContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<Product>? Products { get; set; }
    public DbSet<Address>? Addresses { get; set; }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
}