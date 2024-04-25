using DelTSZ.Models.Enums;
using DelTSZ.Models.Products.ComponentProducts;

namespace DelTSZ.Repositories.ComponentProductRepository;

public interface IComponentProductRepository
{
    Task<IEnumerable<ComponentProductResponse?>> GetAllOwnerComponentProducts();
    void AddComponentProductToUser(ComponentProductRequest product, string id);
    Task<ComponentProduct?> GetOldestComponentProduct(ComponentProductType type);
    void UpdateComponentProduct(ComponentProduct product);
    void DeleteComponentProduct(ComponentProduct product);
}