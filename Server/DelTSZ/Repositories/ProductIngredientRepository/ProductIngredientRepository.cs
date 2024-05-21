using DelTSZ.Data;
using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;
using DelTSZ.Repositories.IngredientRepository;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.ProductIngredientRepository;

public class ProductIngredientRepository(DataContext dataContext, IIngredientRepository ingredientRepository)
    : IProductIngredientRepository
{
    public async Task<List<ProductIngredient>> CreateProductIngredients(ProductRequest productRequest)
    {
        var productDetails = productRequest.Type.GetProductDetails().ToList();
        var ingredients = new List<ProductIngredient>();

        foreach (var pd in productDetails)
        {
            var demandAmount = pd.Item1 * productRequest.Amount;
            var ingredientAmount = 0m;

            do
            {
                var oldestIngredient = await ingredientRepository.GetOwnerOldestIngredientByType(pd.Item2);
                if (oldestIngredient!.Amount - pd.Item1 <= 0)
                {
                    if (ingredients.Exists(pi =>
                            pi.Received == oldestIngredient.Received && pi.Type == oldestIngredient.Type))
                    {
                        ingredients.FirstOrDefault(pi =>
                                pi.Received == oldestIngredient.Received && pi.Type == oldestIngredient.Type)!
                            .Amount += pd.Item1;
                    }
                    else
                    {
                        ingredients.Add(new ProductIngredient
                        {
                            Amount = oldestIngredient.Amount,
                            Received = oldestIngredient.Received,
                            Type = oldestIngredient.Type
                        });
                    }

                    ingredientAmount += oldestIngredient.Amount;
                    await ingredientRepository.DeleteIngredient(oldestIngredient);
                }
                else
                {
                    if (ingredients.Exists(pi =>
                            pi.Received == oldestIngredient.Received && pi.Type == oldestIngredient.Type))
                    {
                        ingredients.FirstOrDefault(pi =>
                                pi.Received == oldestIngredient.Received && pi.Type == oldestIngredient.Type)!
                            .Amount += pd.Item1;
                    }
                    else
                    {
                        ingredients.Add(new ProductIngredient
                        {
                            Amount = pd.Item1,
                            Received = oldestIngredient.Received,
                            Type = oldestIngredient.Type
                        });
                    }

                    ingredientAmount += pd.Item1;
                    oldestIngredient.Amount -= pd.Item1;
                    await ingredientRepository.UpdateIngredient(oldestIngredient);
                }
            } while (ingredientAmount < demandAmount);
        }

        return ingredients;
    }

    public async Task IncreaseProductIngredientsFromOwnerIngredients(Product product, int amount)
    {
        var productDetails = product.Type.GetProductDetails().ToList();

        foreach (var pd in productDetails)
        {
            var demandAmount = pd.Item1 * amount;
            var balance = 0m;

            do
            {
                var oldestIngredient = await ingredientRepository.GetOwnerOldestIngredientByType(pd.Item2);
                var productIngredient =
                    await GetProductIngredientByProductId_Type_Received(product.Id, pd.Item2,
                        oldestIngredient!.Received);

                if (oldestIngredient.Amount - demandAmount <= 0)
                {
                    if (productIngredient != null)
                    {
                        productIngredient.Amount += oldestIngredient.Amount;
                        await UpdateProductIngredient(productIngredient);
                    }
                    else
                    {
                        await CreateProductIngredient(new ProductIngredient
                        {
                            Amount = oldestIngredient.Amount,
                            Received = oldestIngredient.Received,
                            Type = oldestIngredient.Type,
                            ProductId = product.Id
                        });
                    }

                    balance += oldestIngredient.Amount;
                    demandAmount -= balance;
                    await ingredientRepository.DeleteIngredient(oldestIngredient);
                }
                else
                {
                    if (productIngredient != null)
                    {
                        productIngredient.Amount += demandAmount;
                        await UpdateProductIngredient(productIngredient);
                    }
                    else
                    {
                        await CreateProductIngredient(new ProductIngredient
                        {
                            Amount = demandAmount,
                            Received = oldestIngredient.Received,
                            Type = oldestIngredient.Type,
                            ProductId = product.Id
                        });
                    }

                    oldestIngredient.Amount -= demandAmount;
                    demandAmount = 0;

                    await ingredientRepository.UpdateIngredient(oldestIngredient);
                }
            } while (demandAmount > 0);
        }
    }


    private async Task<ProductIngredient?> GetProductIngredientByProductId_Type_Received(int id, IngredientType type,
        DateTime received)
    {
        return await dataContext.ProductIngredients
            .Where(pi => pi.ProductId == id)
            .Where(pi => pi.Type == type && pi.Received == received)
            .FirstOrDefaultAsync();
    }

    private async Task CreateProductIngredient(ProductIngredient productIngredient)
    {
        dataContext.Add(productIngredient);
        await dataContext.SaveChangesAsync();
    }

    private async Task UpdateProductIngredient(ProductIngredient productIngredient)
    {
        dataContext.Update(productIngredient);
        await dataContext.SaveChangesAsync();
    }
}