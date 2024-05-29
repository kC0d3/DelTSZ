using DelTSZ.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace DelTSZ.Services.Authentication;

public interface IAuthService
{
    Task<IdentityResult> RegisterCostumer(Registration registration);
    Task<IdentityResult> RegisterProducer(Registration registration);
    Task<IdentityResult> DeleteUser(User user);
    Task<User?> FindUserById(string id);
    Task<User?> FindUserByEmail(string email);
    Task<SignInResult> Login(User user, string password);
    Task Logout();
}