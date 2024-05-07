using DelTSZ.Models.Components;
using DelTSZ.Models.Enums;

namespace DelTSZ.Repositories.ComponentRepository;

public interface IComponentRepository
{
    Task<IEnumerable<ComponentResponse?>> GetAllOwnerComponents();
    Task<IEnumerable<ComponentResponse?>> GetAllOwnerComponentsByType(ComponentType type);
    Task<decimal> GetAllOwnerComponentAmountsByType(ComponentType type);
    Task<Component?> GetOwnerOldestComponentByType(ComponentType type);
    void CreateComponentToUser(ComponentRequest component, string id, int days);
    Task<Component?> GetComponentById(int id);
    void UpdateComponent(Component component);
    void DeleteComponent(Component component);
}