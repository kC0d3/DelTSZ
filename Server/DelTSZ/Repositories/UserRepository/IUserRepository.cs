using DelTSZ.Models.Users;

namespace DelTSZ.Repositories.UserRepository;

public interface IUserRepository
{
    Task<User?> GetOwner();
}