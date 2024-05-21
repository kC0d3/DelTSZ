﻿using DelTSZ.Data;
using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;
using DelTSZ.Repositories.ProductIngredientRepository;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.ProductRepository;

public class ProductRepository(DataContext dataContext, IProductIngredientRepository productIngredientRepository)
    : IProductRepository
{
    public async Task<IEnumerable<ProductResponse?>> GetAllOwnerProducts()
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Products
            .Where(p => p.UserId == user!.Id)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Type = p.Type,
                Received = p.Packed,
                Amount = p.Amount,
            }).ToListAsync();
    }

    public async Task<int> GetAllOwnerProductsAmountsByType(ProductType type)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Products
            .Where(c => c.UserId == user!.Id && c.Type == type)
            .SumAsync(c => c.Amount);
    }

    public async Task<Product?> GetProductByUserId_Type_PackedDate(ProductType type, string id, int days)
    {
        return await dataContext.Products
            .Where(p => p.UserId == id && p.Type == type && p.Packed == DateTime.Today.AddDays(days))
            .FirstOrDefaultAsync();
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await dataContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateProductToUser(ProductRequest product, string id, IEnumerable<ProductIngredient> components,
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
                        await productIngredientRepository.CreateProductIngredients(ownerProduct, ownerProduct.Amount);

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
                        await productIngredientRepository.CreateProductIngredients(ownerProduct, demandAmount,
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
                        await productIngredientRepository.CreateProductIngredients(ownerProduct, demandAmount);

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
                        await productIngredientRepository.CreateProductIngredients(ownerProduct, demandAmount,
                            userProduct);
                    await UpdateProduct(userProduct);
                }

                ownerProduct.Amount -= demandAmount;
                await UpdateProduct(ownerProduct);
                demandAmount = 0;
            }
        }
    }

    public async Task UpdateProduct(Product product)
    {
        dataContext.Update(product);
        await dataContext.SaveChangesAsync();
    }

    public async Task DeleteProduct(Product product)
    {
        dataContext.Remove(product);
        await dataContext.SaveChangesAsync();
    }

    //Private methods

    private async Task<Product?> GetOwnerOldestProductByType(ProductType type)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
        return await dataContext.Products
            .Where(p => p.UserId == user!.Id && p.Type == type)
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
}