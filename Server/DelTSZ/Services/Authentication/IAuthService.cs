using DelTSZ.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace DelTSZ.Services.Authentication;

public interface IAuthService
{
    Task<IdentityResult> RegisterCustomer(Registration registration);
    Task<IdentityResult> RegisterProducer(Registration registration);
    Task<IdentityResult> UpdateUser(UserUpdateRequest userUpdateRequest, User user);
    Task<IdentityResult> DeleteUser(User user);
    Task<IdentityResult> ChangePassword(User user, string? currentPassword, string? newPassword);
    Task<bool> CheckPassword(User user, string password);
    Task<User?> FindUserById(string id);
    Task<User?> FindUserByEmail(string email);
    Task<SignInResult> Login(User user, string password);
    Task Logout();
}