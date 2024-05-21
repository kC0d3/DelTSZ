using DelTSZ.Data;
using DelTSZ.Models.Components;
using DelTSZ.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.ComponentRepository;

public class ComponentRepository(DataContext dataContext) : IComponentRepository
{
    public async Task<IEnumerable<ComponentSumResponse?>> GetAllOwnerComponentsSumByType()
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Components?
            .Where(c => c.UserId == user!.Id)
            .GroupBy(c => c.Type)
            .Select(g => new ComponentSumResponse
            {
                Type = g.Key,
                Amount = g.Sum(c => c.Amount)
            })
            .ToListAsync()!;
    }

    public async Task<decimal> GetAllOwnerComponentAmountsByType(ComponentType type)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Components?
            .Where(c => c.UserId == user!.Id && c.Type == type)
            .SumAsync(c => c.Amount)!;
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

    public async Task<Component?> GetComponentByUserIdTypeReceivedDate(ComponentType type, string id, int days)
    {
        return await dataContext.Components!
            .Where(c => c.UserId == id && c.Type == type && c.Received == DateTime.Today.AddDays(days))
            .FirstOrDefaultAsync();
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

    public async void ComponentUpdateByRequestAmount(ComponentType type, string id, decimal amount)
    {
        var demandAmount = amount;
        var balance = 0m;

        do
        {
            var component = await GetOwnerOldestComponentByType(type);

            if (component!.Amount - demandAmount <= 0)
            {
                demandAmount -= component.Amount;
                balance += component.Amount;

                var userComp =
                    await GetComponentByUserIdTypeReceivedDate(type, id, component.Received);

                if (userComp == null)
                {
                    CreateComponentToUser(
                        new ComponentUpdateRequest
                            { Type = type, Amount = component.Amount, Received = component.Received },
                        id);
                }
                else
                {
                    userComp.Amount += balance;
                    UpdateComponent(userComp);
                }

                DeleteComponent(component);
            }
            else
            {
                var userComp =
                    await GetComponentByUserIdTypeReceivedDate(type, id, component.Received);

                if (userComp == null)
                {
                    CreateComponentToUser(
                        new ComponentUpdateRequest
                            { Type = type, Amount = demandAmount, Received = component.Received },
                        id);
                }
                else
                {
                    userComp.Amount += amount;
                    UpdateComponent(userComp);
                }

                demandAmount -= amount - balance;
                component.Amount -= amount - balance;
                UpdateComponent(component);
            }
        } while (demandAmount > 0);
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

    private async Task<Component?> GetComponentByUserIdTypeReceivedDate(ComponentType type, string id,
        DateTime received)
    {
        return await dataContext.Components!
            .Where(c => c.UserId == id && c.Type == type && c.Received == received)
            .FirstOrDefaultAsync();
    }

    private void CreateComponentToUser(ComponentUpdateRequest component, string id)
    {
        dataContext.Add(new Component
        {
            Type = component.Type,
            Amount = component.Amount,
            Received = component.Received,
            UserId = id
        });
        dataContext.SaveChanges();
    }
}