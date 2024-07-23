using DelTSZ.Models.Users;

namespace DelTSZ.Repositories.UserRepository;

public interface IUserRepository
{
    Task<User?> GetOwner();
    Task<IEnumerable<User?>> GetProducers();
    Task<UserResponse?> GetUserAllDataById(string id);
    Task<User?> GetUserWithAddressById(string id);
    Task<IEnumerable<UserResponse>> GetProducerUserResponses();
    Task<IEnumerable<UserResponse>> GetCustomerUserResponses();
}