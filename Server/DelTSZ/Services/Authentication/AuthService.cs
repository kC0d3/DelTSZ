using DelTSZ.Models.Addresses;
using DelTSZ.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace DelTSZ.Services.Authentication;

public class AuthService(UserManager<User> userManager) : IAuthService
{
    public async Task<IdentityResult> RegisterCostumerAsync(RegistrationRequest request)
    {
        var user = new User()
        {
            UserName = request.Username,
            Email = request.Email,
            CompanyName = request.Companyname,
            Role = "Costumer",
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
}