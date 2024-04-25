using DelTSZ.Models.Addresses;
using DelTSZ.Models.Products.ComponentProducts;
using DelTSZ.Models.Products.CompositeProducts;
using DelTSZ.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Data;

public class DataContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<ComponentProduct>? ComponentProducts { get; set; }
    public DbSet<CompositeProduct>? CompositeProducts { get; set; }
    public DbSet<Address>? Addresses { get; set; }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ComponentProduct>()
            .HasOne(p => p.User)
            .WithMany(u => u.ComponentProducts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CompositeProduct>()
            .HasOne(p => p.User)
            .WithMany(u => u.CompositeProducts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CompositeProduct>()
            .HasMany(p => p.Components);
        
        builder.Entity<User>()
            .HasOne(u => u.Address)
            .WithOne(a => a.User)
            .HasForeignKey<Address>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasMany(u => u.ComponentProducts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasMany(u => u.CompositeProducts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}