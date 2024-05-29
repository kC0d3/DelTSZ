using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;

namespace DelTSZ.Repositories.ProductIngredientRepository;

public interface IProductIngredientRepository
{
    Task<List<ProductIngredient>> CreateProductIngredientsFromOwnerIngredients(ProductRequest productRequest);
    Task<List<ProductIngredient>> CreateProductIngredientsFromProductIngredients(Product product, int amount);
    Task<List<ProductIngredient>> UpgradeProductIngredientsFromProductIngredients(Product product, int amount, Product userProduct);
    Task IncreaseProductIngredientsFromOwnerIngredients(Product product, int amount);
}