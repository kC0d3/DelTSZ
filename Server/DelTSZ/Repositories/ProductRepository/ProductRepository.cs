using DelTSZ.Data;
using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;
using DelTSZ.Repositories.IngredientRepository;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.ProductRepository;

public class ProductRepository(DataContext dataContext, IIngredientRepository ingredientRepository) : IProductRepository
{
    public async Task<IEnumerable<ProductResponse?>> GetAllOwnerProducts()
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Products
            .Where(p => p.UserId == user!.Id)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Type = p.Type,
                Received = p.Packed,
                Amount = p.Amount,
            }).ToListAsync();
    }

    public async Task<int> GetAllOwnerProductsAmountsByType(ProductType type)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Products
            .Where(c => c.UserId == user!.Id && c.Type == type)
            .SumAsync(c => c.Amount);
    }

    public async Task<Product?> GetProductByUserId_Type_PackedDate(ProductType type, string id, int days)
    {
        return await dataContext.Products
            .Where(p => p.UserId == id && p.Type == type && p.Packed == DateTime.Today.AddDays(days))
            .FirstOrDefaultAsync();
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await dataContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateProductToUser(ProductRequest product, string id, IEnumerable<ProductIngredient> components,
        int days)
    {
        dataContext.Add(new Product
        {
            Type = product.Type,
            Packed = DateTime.Today.AddDays(days),
            Amount = product.Amount,
            Components = components.Select(pc => new ProductIngredient
            {
                Type = pc.Type,
                Received = pc.Received,
                Amount = pc.Amount,
            }).ToList(),
            UserId = id
        });
        await dataContext.SaveChangesAsync();
    }

    public async Task UpdateProduct(Product product)
    {
        dataContext.Update(product);
        await dataContext.SaveChangesAsync();
    }

    public async Task DeleteProduct(Product product)
    {
        dataContext.Remove(product);
        await dataContext.SaveChangesAsync();
    }

    //Private methods

    private async Task<Product?> GetOwnerOldestProductByType(ProductType type)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Products
            .Where(p => p.UserId == user!.Id && p.Type == type)
            .OrderBy(p => p.Packed)
            .FirstOrDefaultAsync();
    }
}