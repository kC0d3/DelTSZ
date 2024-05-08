using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductComponents;
using DelTSZ.Models.Products;

namespace DelTSZ.Repositories.ProductRepository;

public interface IProductRepository
{
    Task<IEnumerable<ProductResponse?>> GetAllOwnerProducts();
    void CreateProductToUser(ProductRequest product, string id, IEnumerable<ProductComponent> components);
    Task<decimal> GetAllOwnerProductsAmountsByType(ProductType type);

    Task<List<ProductComponent>> CreateProductComponents(ComponentType type, decimal amount,
        decimal demandAmount);

    Task<Product?> GetProductById(int id);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product); 
}