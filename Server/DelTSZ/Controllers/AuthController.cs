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
            result = await authService.RegisterCostumer(request);

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
            result = await authService.RegisterProducer(request);

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

    [HttpDelete, Authorize(Roles = "Producer, Costumer")]
    public async Task<IActionResult> DeleteUser()
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;
            if (id == null)
                return Conflict("Wrong user id.");

            var user = await authService.FindUserById(id);
            if (user == null)
                return NotFound("User not found.");

            var result = await authService.DeleteUser(user);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(new { message = "Delete successful.", result });
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong, please try again.");
        }
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