using DelTSZ.Models.Components;
using DelTSZ.Models.Enums;

namespace DelTSZ.Repositories.ComponentRepository;

public interface IComponentRepository
{
    Task<IEnumerable<ComponentSumResponse?>> GetAllOwnerComponentsSumByType();
    Task<decimal> GetAllOwnerComponentAmountsByType(ComponentType type);
    Task<Component?> GetOwnerOldestComponentByType(ComponentType type);
    Task<Component?> GetComponentById(int id);
    Task<Component?> GetComponentByUserIdTypeReceivedDate(ComponentType type, string id, int days);
    void CreateComponentToUser(ComponentRequest component, string id, int days);
    void ComponentUpdateByRequestAmount(ComponentType type, string id, decimal amount);
    void UpdateComponent(Component component);
    void DeleteComponent(Component component);
}