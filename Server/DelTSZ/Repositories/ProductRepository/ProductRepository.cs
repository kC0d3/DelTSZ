using DelTSZ.Data;
using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;
using DelTSZ.Repositories.ProductIngredientRepository;
using DelTSZ.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.ProductRepository;

public class ProductRepository(
    DataContext dataContext,
    IProductIngredientRepository productIngredientRepository,
    IUserRepository userRepository)
    : IProductRepository
{
    public async Task<IEnumerable<ProductSumResponse?>> GetAllOwnerProductsSumByType()
    {
        var owner = await userRepository.GetOwner();
        return await dataContext.Products
            .Where(p => p.UserId == owner!.Id)
            .GroupBy(p => p.Type)
            .Select(g => new ProductSumResponse
            {
                Type = g.Key,
                Amount = g.Sum(p => p.Amount)
            })
            .ToListAsync();
    }

    public async Task<int> GetAllOwnerProductsAmountsByType(ProductType type)
    {
        var owner = await userRepository.GetOwner();
        return await dataContext.Products
            .Where(c => c.UserId == owner!.Id && c.Type == type)
            .SumAsync(c => c.Amount);
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await dataContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task ProductUpdateByRequestAmount(ProductType type, string id, int amount)
    {
        var demandAmount = amount;

        while (demandAmount > 0)
        {
            var ownerProduct = await GetOwnerOldestProductByType(type);

            if (ownerProduct!.Amount <= demandAmount)
            {
                demandAmount -= ownerProduct.Amount;
                var userProduct = await GetProductByUserId_Type_PackedDate(type, id, ownerProduct.Packed);

                if (userProduct == null)
                {
                    var ingredients =
                        await productIngredientRepository.CreateProductIngredientsFromProductIngredients(ownerProduct,
                            ownerProduct.Amount);

                    await CreateProductToUser(new ProductRequest
                    {
                        Type = type,
                        Amount = ownerProduct.Amount
                    }, id, ownerProduct.Packed, ingredients);
                }
                else
                {
                    userProduct.Amount += ownerProduct.Amount;
                    userProduct.Ingredients =
                        await productIngredientRepository.UpgradeProductIngredientsFromProductIngredients(ownerProduct,
                            demandAmount,
                            userProduct);
                    await UpdateProduct(userProduct);
                }

                await DeleteProduct(ownerProduct);
            }
            else
            {
                var userProduct = await GetProductByUserId_Type_PackedDate(type, id, ownerProduct.Packed);

                if (userProduct == null)
                {
                    var ingredients =
                        await productIngredientRepository.CreateProductIngredientsFromProductIngredients(ownerProduct,
                            demandAmount);

                    await CreateProductToUser(new ProductRequest
                    {
                        Type = type,
                        Amount = demandAmount
                    }, id, ownerProduct.Packed, ingredients);
                }
                else
                {
                    userProduct.Amount += demandAmount;
                    userProduct.Ingredients =
                        await productIngredientRepository.UpgradeProductIngredientsFromProductIngredients(ownerProduct,
                            demandAmount,
                            userProduct);
                    await UpdateProduct(userProduct);
                }

                ownerProduct.Amount -= demandAmount;
                await UpdateProduct(ownerProduct);
                demandAmount = 0;
            }
        }
    }

    public async Task CreateOrUpdateProduct(ProductRequest productRequest, string id, int days)
    {
        var product = await GetProductByUserId_Type_PackedDate(productRequest.Type, id, days);
        var ingredients = new List<ProductIngredient>();

        if (product == null)
        {
            ingredients.AddRange(
                await productIngredientRepository.CreateProductIngredientsFromOwnerIngredients(productRequest));
            await CreateProductToUser(productRequest, id, ingredients, days);
        }
        else
        {
            product.Amount += productRequest.Amount;
            await productIngredientRepository.IncreaseProductIngredientsFromOwnerIngredients(product,
                productRequest.Amount);
            await UpdateProduct(product);
        }
    }

    public async Task DeleteProduct(Product product)
    {
        dataContext.Remove(product);
        await dataContext.SaveChangesAsync();
    }

    //Private methods

    private async Task<Product?> GetOwnerOldestProductByType(ProductType type)
    {
        var owner = await userRepository.GetOwner();
        return await dataContext.Products
            .Where(p => p.UserId == owner!.Id && p.Type == type)
            .OrderBy(p => p.Packed)
            .FirstOrDefaultAsync();
    }

    private async Task<Product?> GetProductByUserId_Type_PackedDate(ProductType type, string id,
        DateTime packed)
    {
        return await dataContext.Products
            .Where(p => p.UserId == id && p.Type == type && p.Packed == packed)
            .FirstOrDefaultAsync();
    }

    private async Task<Product?> GetProductByUserId_Type_PackedDate(ProductType type, string id, int days)
    {
        return await dataContext.Products
            .Where(p => p.UserId == id && p.Type == type && p.Packed == DateTime.Today.AddDays(days))
            .FirstOrDefaultAsync();
    }

    private async Task CreateProductToUser(ProductRequest product, string id, IEnumerable<ProductIngredient> components,
        int days)
    {
        dataContext.Add(new Product
        {
            Type = product.Type,
            Packed = DateTime.Today.AddDays(days),
            Amount = product.Amount,
            Ingredients = components.Select(pc => new ProductIngredient
            {
                Type = pc.Type,
                Received = pc.Received,
                Amount = pc.Amount,
            }).ToList(),
            UserId = id
        });
        await dataContext.SaveChangesAsync();
    }

    private async Task CreateProductToUser(ProductRequest product, string id, DateTime packed,
        IEnumerable<ProductIngredient> components)
    {
        dataContext.Add(new Product
        {
            Type = product.Type,
            Packed = packed,
            Amount = product.Amount,
            Ingredients = components.Select(pc => new ProductIngredient
            {
                Type = pc.Type,
                Received = pc.Received,
                Amount = pc.Amount,
            }).ToList(),
            UserId = id
        });
        await dataContext.SaveChangesAsync();
    }

    private async Task UpdateProduct(Product product)
    {
        dataContext.Update(product);
        await dataContext.SaveChangesAsync();
    }
}