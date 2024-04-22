using DelTSZ.Models.Users;
using DelTSZ.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(Registration request)
    {
        IdentityResult result;

        try
        {
            result = await authService.RegisterCostumerAsync(request);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong, please try again.");
        }

        return Ok(new { message = "Register successfully.", result });
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(Login login)
    {
        try
        {
            var user = await authService.FindUserByEmail(login.Email);

            if (user == null)
            {
                return Unauthorized("Check your login credentials and try again");
            }

            var result = await authService.Login(user, login.Password);

            if (!result.Succeeded)
            {
                return Unauthorized("Check your login credentials and try again");
            }
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong, please try again.");
        }

        return Ok(new { message = "Login successful." });
    }
}