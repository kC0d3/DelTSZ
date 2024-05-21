using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;

namespace DelTSZ.Repositories.ProductIngredientRepository;

public interface IProductIngredientRepository
{
    Task<List<ProductIngredient>> CreateProductIngredients(ProductRequest productRequest);
}