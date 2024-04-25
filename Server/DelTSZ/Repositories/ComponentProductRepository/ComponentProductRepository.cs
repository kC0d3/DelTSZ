using DelTSZ.Data;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Products.ComponentProducts;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.ComponentProductRepository;

public class ComponentProductRepository(DataContext dataContext) : IComponentProductRepository
{
    public async Task<IEnumerable<ComponentProductResponse?>> GetAllOwnerComponentProducts()
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.ComponentProducts?.Select(p => new ComponentProductResponse()
        {
            Id = p.Id,
            ProductType = p.ProductType,
            Received = p.Received,
            Amount = p.Amount,
            UserId = p.UserId
        }).Where(p => p.UserId == user!.Id).ToListAsync()!;
    }
}