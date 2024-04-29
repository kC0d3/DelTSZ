using DelTSZ.Models.Components;
using DelTSZ.Models.Enums;

namespace DelTSZ.Repositories.ComponentRepository;

public interface IComponentRepository
{
    Task<IEnumerable<ComponentResponse?>> GetAllOwnerComponents();
    void AddComponentToUser(ComponentRequest product, string id);
    Task<Component?> GetOldestComponent(ComponentType type);
    Task<Component?> GetComponentById(int id);
    void UpdateComponent(Component product);
    void DeleteComponent(Component product);
}