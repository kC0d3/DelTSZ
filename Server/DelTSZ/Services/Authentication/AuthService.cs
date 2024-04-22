using DelTSZ.Models.Addresses;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace DelTSZ.Services.Authentication;

public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager) : IAuthService
{
    public async Task<IdentityResult> RegisterCostumerAsync(Registration request)
    {
        var user = new User()
        {
            UserName = request.Username,
            Email = request.Email,
            CompanyName = request.Companyname,
            Role = Roles.Costumer.ToString(),
            Address = new Address
            {
                ZipCode = request.Address.ZipCode,
                City = request.Address.City,
                Street = request.Address.Street,
                HouseNumber = request.Address.HouseNumber
            }
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return result;
        }

        await userManager.AddToRoleAsync(user, user.Role);
        return result;
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<SignInResult> Login(User user, string password)
    {
        return await signInManager.PasswordSignInAsync(user, password, false, false);
    }

    public async void Logout()
    {
        await signInManager.SignOutAsync();
    }
}