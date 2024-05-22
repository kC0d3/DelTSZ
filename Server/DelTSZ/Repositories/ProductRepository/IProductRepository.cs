using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductIngredients;
using DelTSZ.Models.Products;

namespace DelTSZ.Repositories.ProductRepository;

public interface IProductRepository
{
    Task<IEnumerable<ProductSumResponse?>> GetAllOwnerProductsSumByType();
    Task<int> GetAllOwnerProductsAmountsByType(ProductType type);
    Task<Product?> GetProductByUserId_Type_PackedDate(ProductType type, string id, int days);
    Task CreateProductToUser(ProductRequest product, string id, IEnumerable<ProductIngredient> components, int days);
    Task ProductUpdateByRequestAmount(ProductType type, string id, int amount);
    Task<Product?> GetProductById(int id);
    Task UpdateProduct(Product product);
    Task DeleteProduct(Product product);
}