using DelTSZ.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace DelTSZ.Services.Authentication;

public interface IAuthService
{
    public Task<IdentityResult> RegisterCostumerAsync(Registration request);
    public Task<User?> FindUserByEmail(string email);
}