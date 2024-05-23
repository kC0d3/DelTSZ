using DelTSZ.Data;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Ingredients;
using DelTSZ.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.IngredientRepository;

public class IngredientRepository(DataContext dataContext, IUserRepository userRepository) : IIngredientRepository
{
    public async Task<IEnumerable<IngredientSumResponse>> GetAllOwnerIngredientsSumByType()
    {
        var owner = await userRepository.GetOwner();
        return await dataContext.Ingredients
            .Where(i => i.UserId == owner!.Id)
            .GroupBy(i => i.Type)
            .Select(g => new IngredientSumResponse
            {
                Type = g.Key,
                Amount = g.Sum(c => c.Amount)
            })
            .ToListAsync();
    }

    public async Task<decimal> GetAllOwnerIngredientAmountsByType(IngredientType type)
    {
        var owner = await userRepository.GetOwner();
        return await dataContext.Ingredients
            .Where(i => i.UserId == owner!.Id && i.Type == type)
            .SumAsync(i => i.Amount);
    }

    public async Task<Ingredient?> GetOwnerOldestIngredientByType(IngredientType type)
    {
        var owner = await userRepository.GetOwner();
        return await dataContext.Ingredients
            .Where(i => i.UserId == owner!.Id && i.Type == type)
            .OrderBy(i => i.Received)
            .FirstOrDefaultAsync();
    }

    public async Task<Ingredient?> GetIngredientById(int id)
    {
        return await dataContext.Ingredients.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Ingredient?> GetIngredientByUserId_Type_ReceivedDate(IngredientType type, string id, int days)
    {
        return await dataContext.Ingredients
            .Where(i => i.UserId == id && i.Type == type && i.Received == DateTime.Today.AddDays(days))
            .FirstOrDefaultAsync();
    }

    public async Task CreateIngredientToUser(IngredientRequest ingredient, string id, int days)
    {
        dataContext.Add(new Ingredient
        {
            Type = ingredient.Type,
            Amount = ingredient.Amount,
            Received = DateTime.Today.AddDays(days),
            UserId = id
        });
        await dataContext.SaveChangesAsync();
    }

    public async Task IngredientUpdateByRequestAmount(IngredientType type, string id, decimal amount)
    {
        var demandAmount = amount;
        var balance = 0m;

        do
        {
            var ownerIngredient = await GetOwnerOldestIngredientByType(type);

            if (ownerIngredient!.Amount - demandAmount <= 0)
            {
                demandAmount -= ownerIngredient.Amount;
                balance += ownerIngredient.Amount;

                var userIngredient =
                    await GetIngredientByUserId_Type_ReceivedDate(type, id, ownerIngredient.Received);

                if (userIngredient == null)
                {
                    await CreateIngredientToUser(
                        new IngredientUpdateRequest
                            { Type = type, Amount = ownerIngredient.Amount, Received = ownerIngredient.Received },
                        id);
                }
                else
                {
                    userIngredient.Amount += balance;
                    await UpdateIngredient(userIngredient);
                }

                await DeleteIngredient(ownerIngredient);
            }
            else
            {
                var userIngredient =
                    await GetIngredientByUserId_Type_ReceivedDate(type, id, ownerIngredient.Received);

                if (userIngredient == null)
                {
                    await CreateIngredientToUser(
                        new IngredientUpdateRequest
                            { Type = type, Amount = demandAmount, Received = ownerIngredient.Received },
                        id);
                }
                else
                {
                    userIngredient.Amount += amount;
                    await UpdateIngredient(userIngredient);
                }

                demandAmount -= amount - balance;
                ownerIngredient.Amount -= amount - balance;
                await UpdateIngredient(ownerIngredient);
            }
        } while (demandAmount > 0);
    }

    public async Task IngredientUpdateById(int id, string userId)
    {
        var ingredient = await GetIngredientById(id);
        var userIngredient =
            await GetIngredientByUserId_Type_ReceivedDate(ingredient!.Type, userId, ingredient.Received);

        if (userIngredient == null)
        {
            ingredient.UserId = userId;
            await UpdateIngredient(ingredient);
        }
        else
        {
            userIngredient.Amount += ingredient.Amount;
            await UpdateIngredient(userIngredient);
            await DeleteIngredient(ingredient);
        }
    }

    public async Task UpdateIngredient(Ingredient ingredient)
    {
        dataContext.Update(ingredient);
        await dataContext.SaveChangesAsync();
    }

    public async Task DeleteIngredient(Ingredient ingredient)
    {
        dataContext.Remove(ingredient);
        await dataContext.SaveChangesAsync();
    }

    //Private methods
    
    private async Task<Ingredient?> GetIngredientByUserId_Type_ReceivedDate(IngredientType type, string id,
        DateTime received)
    {
        return await dataContext.Ingredients
            .Where(i => i.UserId == id && i.Type == type && i.Received == received)
            .FirstOrDefaultAsync();
    }

    private async Task CreateIngredientToUser(IngredientUpdateRequest ingredient, string id)
    {
        dataContext.Add(new Ingredient
        {
            Type = ingredient.Type,
            Amount = ingredient.Amount,
            Received = ingredient.Received,
            UserId = id
        });
        await dataContext.SaveChangesAsync();
    }
}