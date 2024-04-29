using DelTSZ.Models.Addresses;
using DelTSZ.Models.Components;
using DelTSZ.Models.Products;
using DelTSZ.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Data;

public class DataContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<Component>? Components { get; set; }
    public DbSet<Product>? Products { get; set; }
    public DbSet<Address>? Addresses { get; set; }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .HasOne(p => p.User)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Product>()
            .HasMany(p => p.Components)
            .WithOne(c => c.Product)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Component>()
            .HasOne(c => c.Product)
            .WithMany(p => p.Components)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Entity<Component>()
            .HasOne(c => c.User)
            .WithMany(u=> u.Components)
            .HasForeignKey(c=> c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasOne(u => u.Address)
            .WithOne(a => a.User)
            .HasForeignKey<Address>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasMany(u => u.Products)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasMany(u => u.Components)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}