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
}