using DelTSZ.Models.Addresses;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace DelTSZ.Services.Authentication;

public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager) : IAuthService
{
    public async Task<IdentityResult> RegisterCustomer(Registration registration)
    {
        var user = new User
        {
            UserName = registration.UserName,
            Email = registration.Email,
            CompanyName = registration.CompanyName,
            Role = Roles.Customer.ToString(),
            Address = new Address
            {
                ZipCode = registration.Address!.ZipCode,
                City = registration.Address.City,
                Street = registration.Address.Street,
                HouseNumber = registration.Address.HouseNumber
            }
        };

        var result = await userManager.CreateAsync(user, registration.Password!);

        if (!result.Succeeded)
            return result;

        await userManager.AddToRoleAsync(user, user.Role);
        return result;
    }

    public async Task<IdentityResult> RegisterProducer(Registration registration)
    {
        var user = new User
        {
            UserName = registration.UserName,
            Email = registration.Email,
            CompanyName = registration.CompanyName,
            Role = Roles.Producer.ToString(),
            Address = new Address
            {
                ZipCode = registration.Address!.ZipCode,
                City = registration.Address.City,
                Street = registration.Address.Street,
                HouseNumber = registration.Address.HouseNumber
            }
        };

        var result = await userManager.CreateAsync(user, registration.Password!);

        if (!result.Succeeded)
            return result;

        await userManager.AddToRoleAsync(user, user.Role);
        return result;
    }

    public async Task<IdentityResult> UpdateUser(UserUpdateRequest userUpdateRequest, User user)
    {
        user.Email = userUpdateRequest.Email;
        user.UserName = userUpdateRequest.UserName;
        user.CompanyName = userUpdateRequest.CompanyName;
        user.Address!.ZipCode = userUpdateRequest.Address!.ZipCode;
        user.Address.City = userUpdateRequest.Address.City;
        user.Address.Street = userUpdateRequest.Address.Street;
        user.Address.HouseNumber = userUpdateRequest.Address.HouseNumber;

        return await userManager.UpdateAsync(user);
    }
    
    public async Task<IdentityResult> DeleteUser(User user)
    {
        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return result;

        await userManager.RemoveFromRoleAsync(user, user.Role!);
        return result;
    }

    public async Task<IdentityResult> ChangePassword(User user, string? currentPassword, string? newPassword)
    {
        return await userManager.ChangePasswordAsync(user, currentPassword!, newPassword!);
    }
    
    public async Task<bool> CheckPassword(User user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }
    
    public async Task<User?> FindUserById(string id)
    {
        return await userManager.FindByIdAsync(id);
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<SignInResult> Login(User user, string password)
    {
        return await signInManager.PasswordSignInAsync(user, password, false, false);
    }

    public async Task Logout()
    {
        await signInManager.SignOutAsync();
    }
}