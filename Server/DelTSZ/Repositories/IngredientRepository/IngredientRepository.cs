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

    public async Task<IEnumerable<IngredientResponse>> GetAllProducerIngredientAmountsByType()
    {
        var producers = await userRepository.GetProducers();
        var producerIds = producers.Select(u => u!.Id).ToList();

        return await dataContext.Ingredients
            .Where(i => producerIds.Contains(i.UserId!))
            .Select(i => new IngredientResponse
            {
                Id = i.Id,
                Amount = i.Amount,
                Received = i.Received,
                Type = i.Type
            })
            .ToListAsync();
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

    public async Task IngredientUpdateByRequestAmount(IngredientType type, string userId, decimal requestedAmount)
    {
        var remainingAmount = requestedAmount;

        while (remainingAmount > 0)
        {
            var oldestIngredient = await GetOwnerOldestIngredientByType(type);
            var transferableAmount = Math.Min(oldestIngredient!.Amount, remainingAmount);

            await TransferIngredient(type, userId, oldestIngredient, transferableAmount);

            remainingAmount -= transferableAmount;

            if (oldestIngredient.Amount == 0)
            {
                await DeleteIngredient(oldestIngredient);
            }
            else
            {
                await UpdateIngredient(oldestIngredient);
            }
        }
    }

    public async Task CreateOrUpdateIngredient(IngredientRequest ingredientRequest, string id, int days)
    {
        var ingredient =
            await GetIngredientByUserId_Type_ReceivedDate(ingredientRequest.Type, id, days);

        if (ingredient == null)
        {
            await CreateIngredientToUser(ingredientRequest, id, days);
        }
        else
        {
            ingredient.Amount += ingredientRequest.Amount;
            await UpdateIngredient(ingredient);
        }
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

    private async Task TransferIngredient(IngredientType type, string userId, Ingredient ownerIngredient,
        decimal amount)
    {
        var userIngredient = await GetIngredientByUserId_Type_ReceivedDate(type, userId, ownerIngredient.Received);

        if (userIngredient == null)
        {
            await CreateIngredientToUser(new IngredientUpdateRequest
            {
                Type = type,
                Amount = amount,
                Received = ownerIngredient.Received
            }, userId);
        }
        else
        {
            userIngredient.Amount += amount;
            await UpdateIngredient(userIngredient);
        }

        ownerIngredient.Amount -= amount;
    }

    private async Task<Ingredient?> GetIngredientByUserId_Type_ReceivedDate(IngredientType type, string id,
        DateTime received)
    {
        return await dataContext.Ingredients
            .Where(i => i.UserId == id && i.Type == type && i.Received == received)
            .FirstOrDefaultAsync();
    }

    private async Task<Ingredient?> GetIngredientByUserId_Type_ReceivedDate(IngredientType type, string id, int days)
    {
        return await dataContext.Ingredients
            .Where(i => i.UserId == id && i.Type == type && i.Received == DateTime.Today.AddDays(days))
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

    private async Task CreateIngredientToUser(IngredientRequest ingredient, string id, int days)
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
}