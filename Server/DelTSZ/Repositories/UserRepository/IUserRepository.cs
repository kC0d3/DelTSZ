using DelTSZ.Models.Users;

namespace DelTSZ.Repositories.UserRepository;

public interface IUserRepository
{
    Task<User?> GetOwner();
    Task<UserResponse?> GetUserById(string id);
    Task<IEnumerable<UserResponse?>> GetProducers();
}