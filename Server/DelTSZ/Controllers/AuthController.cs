using DelTSZ.Models.Users;
using DelTSZ.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(Registration request)
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

    [HttpPost("register/producer"), Authorize(Roles = "Owner")]
    public async Task<IActionResult> RegisterProducer(Registration request)
    {
        IdentityResult result;

        try
        {
            result = await authService.RegisterProducerAsync(request);

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
    public async Task<IActionResult> Login(Login login)
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
    
    [HttpGet("logout"), Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await authService.Logout();
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong, please try again.");
        }

        return Ok(new { message = "Logout successful." });
    }
}