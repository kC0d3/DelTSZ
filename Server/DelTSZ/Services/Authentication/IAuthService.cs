using DelTSZ.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace DelTSZ.Services.Authentication;

public interface IAuthService
{
    Task<IdentityResult> RegisterCostumerAsync(Registration request);
    Task<IdentityResult> RegisterProducerAsync(Registration request);
    Task<User?> FindUserByEmail(string email);
    Task<SignInResult> Login(User user, string password);
    Task Logout();
}