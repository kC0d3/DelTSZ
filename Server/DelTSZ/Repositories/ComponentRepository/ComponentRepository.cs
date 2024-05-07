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
            .Where(c => c.UserId == user!.Id)
            .Select(c => new ComponentResponse
            {
                Id = c.Id,
                Type = c.Type,
                Received = c.Received,
                Amount = c.Amount,
            }).ToListAsync()!;
    }

    public async Task<Component?> GetOwnerOldestComponentByType(ComponentType type)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Components!
            .Where(c => c.UserId == user!.Id && c.Type == type)
            .OrderBy(c => c.Received)
            .FirstOrDefaultAsync();
    }

    public async Task<Component?> GetComponentById(int id)
    {
        return await dataContext.Components!.FirstOrDefaultAsync(c => c.Id == id);
    }

    public void CreateComponentToUser(ComponentRequest component, string id, int days)
    {
        dataContext.Add(new Component
        {
            Type = component.Type,
            Amount = component.Amount,
            Received = DateTime.Today.AddDays(days),
            UserId = id
        });
        dataContext.SaveChanges();
    }

    public void UpdateComponent(Component component)
    {
        dataContext.Update(component);
        dataContext.SaveChanges();
    }

    public void DeleteComponent(Component component)
    {
        dataContext.Remove(component);
        dataContext.SaveChanges();
    }
}