﻿using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;

namespace DelTSZ.Repositories.ProductRepository;

public interface IProductRepository
{
    Task<IEnumerable<ProductResponse?>> GetAllOwnerProducts();
    Task CreateProductToUser(ProductRequest product, string id, IEnumerable<ProductIngredient> components, int days);
    Task<int> GetAllOwnerProductsAmountsByType(ProductType type);
    Task<Product?> GetProductByUserId_Type_PackedDate(ProductType type, string id, int days);
    Task<Product?> GetProductById(int id);
    Task UpdateProduct(Product product);
    Task DeleteProduct(Product product);
}