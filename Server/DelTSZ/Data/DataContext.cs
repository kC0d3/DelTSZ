using DelTSZ.Models.Addresses;
using DelTSZ.Models.Ingredients;
using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;
using DelTSZ.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User, IdentityRole, string>(options)
{
    public DbSet<Ingredient> Ingredients { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<ProductIngredient> ProductIngredients { get; init; }
    public DbSet<Address> Addresses { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .HasOne(p => p.User)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Product>()
            .HasMany(p => p.Ingredients)
            .WithOne(c => c.Product)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Ingredient>()
            .Property(c => c.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Ingredient>()
            .HasOne(c => c.User)
            .WithMany(u => u.Ingredients)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProductIngredient>()
            .Property(c => c.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<ProductIngredient>()
            .HasOne(c => c.Product)
            .WithMany(p => p.Ingredients)
            .HasForeignKey(c => c.ProductId)
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
            .HasMany(u => u.Ingredients)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}