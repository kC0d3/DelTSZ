using System.ComponentModel.DataAnnotations;
using DelTSZ.Models.Users;
using DelTSZ.Repositories.UserRepository;
using DelTSZ.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService, IUserRepository userRepository) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([Required] Registration registration)
    {
        try
        {
            var result = await authService.RegisterCostumer(registration);

            if (!result.Succeeded)
                return Conflict(result);

            return Ok(new { message = "Register successfully.", result });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Something went wrong, please try again." });
        }
    }

    [HttpPost("register/producer"), Authorize(Roles = "Owner")]
    public async Task<IActionResult> RegisterProducer([Required] Registration registration)
    {
        try
        {
            var result = await authService.RegisterProducer(registration);

            if (!result.Succeeded)
                return Conflict(result);

            return Ok(new { message = "Register successfully.", result });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Something went wrong, please try again." });
        }
    }

    [HttpPost("update"), Authorize]
    public async Task<IActionResult> UpdateUser([Required] UserUpdateRequest userUpdateRequest)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;
            if (id == null)
                return Conflict(new { message = "Wrong user id." });

            var user = await userRepository.GetUserWithAddressById(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            var result = await authService.UpdateUser(userUpdateRequest, user);
            if (!result.Succeeded)
                return Conflict(result);

            return Ok(new { message = "User update successful.", result });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Something went wrong, please try again." });
        }
    }

    [HttpPost("delete"), Authorize(Roles = "Producer, Costumer")]
    public async Task<IActionResult> DeleteUser([FromBody] string password)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;
            if (id == null)
                return Conflict(new { message = "Wrong user id." });

            var user = await authService.FindUserById(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            if (!await authService.CheckPassword(user, password))
                return Conflict(new { message = "Incorrect password." });

            var result = await authService.DeleteUser(user);
            if (!result.Succeeded)
                return Conflict(result);

            await authService.Logout();
            return Ok(new { message = "Delete and logout successful.", result });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Something went wrong, please try again." });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([Required] Login login)
    {
        try
        {
            var user = await authService.FindUserByEmail(login.Email);
            if (user == null)
                return Unauthorized(new { message = "Check your login credentials and try again." });

            var result = await authService.Login(user, login.Password);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Check your login credentials and try again." });

            return Ok(new { message = "Login successful." });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Something went wrong, please try again." });
        }
    }

    [HttpGet("logout"), Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await authService.Logout();
            return Ok(new { message = "Logout successful." });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Something went wrong, please try again." });
        }
    }
}