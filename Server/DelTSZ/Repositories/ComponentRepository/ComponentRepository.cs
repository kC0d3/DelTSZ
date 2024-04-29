using DelTSZ.Data;
using DelTSZ.Models.Components;
using DelTSZ.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.ComponentRepository;

public class ComponentRepository(DataContext dataContext) : IComponentRepository
{
    public async Task<IEnumerable<ComponentResponse?>> GetAllOwnerComponents()
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Components?
            .Where(c => c.UserId == user!.Id && c.ProductId == null)
            .Select(c => new ComponentResponse
            {
                Id = c.Id,
                Type = c.Type,
                Received = c.Received,
                Amount = c.Amount,
            }).ToListAsync()!;
    }

    public void AddComponentToUser(ComponentRequest product, string id)
    {
        dataContext.Add(new Component
        {
            Type = product.Type,
            Received = product.Received,
            Amount = product.Amount,
            UserId = id
        });
        dataContext.SaveChanges();
    }

    public async Task<Component?> GetComponentById(int id)
    {
        return await dataContext.Components!.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Component?> GetOldestComponent(ComponentType type)
    {
        return await dataContext.Components!
            .Where(p => p.Type == type)
            .OrderBy(p => p.Received)
            .FirstOrDefaultAsync();
    }

    public void UpdateComponent(Component product)
    {
        dataContext.Update(product);
        dataContext.SaveChanges();
    }

    public void DeleteComponent(Component product)
    {
        dataContext.Remove(product);
        dataContext.SaveChanges();
    }
}