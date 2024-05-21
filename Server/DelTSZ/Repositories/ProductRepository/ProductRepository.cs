using DelTSZ.Data;
using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductComponents;
using DelTSZ.Models.Products;
using DelTSZ.Repositories.ComponentRepository;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.ProductRepository;

public class ProductRepository(DataContext dataContext, IComponentRepository componentRepository) : IProductRepository
{
    public async Task<IEnumerable<ProductResponse?>> GetAllOwnerProducts()
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Products?
            .Where(p => p.UserId == user!.Id)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Type = p.Type,
                Received = p.Packed,
                Amount = p.Amount,
            }).ToListAsync()!;
    }

    public async Task<decimal> GetAllOwnerProductsAmountsByType(ProductType type)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Products?
            .Where(c => c.UserId == user!.Id && c.Type == type)
            .SumAsync(c => c.Amount)!;
    }

    public void CreateProductToUser(ProductRequest product, string id, IEnumerable<ProductComponent> components)
    {
        dataContext.Add(new Product
        {
            Type = product.Type,
            Packed = DateTime.Today,
            Amount = product.Amount,
            Components = components.Select(c => new ProductComponent
            {
                Type = c.Type,
                Received = c.Received,
                Amount = c.Amount,
            }).ToList(),
            UserId = id
        });
        dataContext.SaveChanges();
    }

    public async Task<List<ProductComponent>> CreateProductComponents(ComponentType type, decimal amount,
        decimal demandAmount)
    {
        var componentAmount = 0m;
        var components = new List<ProductComponent>();

        do
        {
            var oldest = await componentRepository.GetOwnerOldestComponentByType(type);
            if (oldest!.Amount - amount <= 0)
            {
                if (components.Exists(c => c.Received == oldest.Received && c.Type == oldest.Type))
                {
                    components.FirstOrDefault(c => c.Received == oldest.Received && c.Type == oldest.Type)!
                        .Amount += amount;
                }
                else
                {
                    components.Add(new ProductComponent
                    {
                        Amount = oldest.Amount,
                        Received = oldest.Received,
                        Type = oldest.Type
                    });
                }

                componentAmount += oldest.Amount;
                componentRepository.DeleteComponent(oldest);
            }
            else
            {
                if (components.Exists(c => c.Received == oldest.Received && c.Type == oldest.Type))
                {
                    components.FirstOrDefault(c => c.Received == oldest.Received && c.Type == oldest.Type)!
                        .Amount += amount;
                }
                else
                {
                    components.Add(new ProductComponent
                    {
                        Amount = amount,
                        Received = oldest.Received,
                        Type = oldest.Type
                    });
                }

                componentAmount += amount;
                oldest.Amount -= amount;
                componentRepository.UpdateComponent(oldest);
            }
        } while (componentAmount < demandAmount);

        return components;
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await dataContext.Products!.FirstOrDefaultAsync(c => c.Id == id);
    }

    public void UpdateProduct(Product product)
    {
        dataContext.Update(product);
        dataContext.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        dataContext.Remove(product);
        dataContext.SaveChanges();
    }

    private async Task<Product?> GetOwnerOldestProductByType(ProductType type)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Products!
            .Where(c => c.UserId == user!.Id && c.Type == type)
            .OrderBy(c => c.Packed)
            .FirstOrDefaultAsync();
    }
}