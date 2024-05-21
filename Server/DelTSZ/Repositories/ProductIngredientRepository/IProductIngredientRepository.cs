using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;

namespace DelTSZ.Repositories.ProductIngredientRepository;

public interface IProductIngredientRepository
{
    Task<List<ProductIngredient>> CreateProductIngredients(ProductRequest productRequest);
    Task<List<ProductIngredient>> CreateProductIngredients(Product product, int amount);
    Task IncreaseProductIngredientsFromOwnerIngredients(Product product, int amount);
    Task UpdateProductIngredient(ProductIngredient productIngredient);
    Task DeleteProductIngredient(ProductIngredient productIngredient);
}