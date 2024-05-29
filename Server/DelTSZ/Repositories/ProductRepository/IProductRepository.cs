using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;

namespace DelTSZ.Repositories.ProductRepository;

public interface IProductRepository
{
    Task<IEnumerable<ProductSumResponse?>> GetAllOwnerProductsSumByType();
    Task<int> GetAllOwnerProductsAmountsByType(ProductType type);
    Task ProductUpdateByRequestAmount(ProductType type, string id, int amount);
    Task<Product?> GetProductById(int id);
    Task DeleteProduct(Product product);
}