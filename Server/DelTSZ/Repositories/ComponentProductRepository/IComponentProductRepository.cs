using DelTSZ.Models.Products.ComponentProducts;

namespace DelTSZ.Repositories.ComponentProductRepository;

public interface IComponentProductRepository
{
    Task<IEnumerable<ComponentProductResponse?>> GetAllOwnerComponentProducts();
    void AddComponentProductToUser(ComponentProductRequest product, string id);
}