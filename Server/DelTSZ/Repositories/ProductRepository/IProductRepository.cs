using DelTSZ.Models.ProductComponents;
using DelTSZ.Models.Products;

namespace DelTSZ.Repositories.ProductRepository;

public interface IProductRepository
{
    Task<IEnumerable<ProductResponse?>> GetAllOwnerProducts();
    void CreateProductToUser(ProductRequest product, string id, IEnumerable<ProductComponent> components);
    
}