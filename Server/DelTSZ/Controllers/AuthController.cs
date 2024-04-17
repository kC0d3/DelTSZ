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
    public async Task<ActionResult> Register(RegistrationRequest request)
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
        catch (Exception ex)
        {
            return BadRequest("Something went wrong, please try again.");
        }

        return Ok(new { message = "Register successfully", result });
    }
}