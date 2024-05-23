using DelTSZ.Models.Users;
using DelTSZ.Repositories.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/users")]
[ApiController]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet, Authorize]
    public async Task<ActionResult<UserResponse>> GetUser()
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("identifier"))?.Value;
            if (id == null)
                return NotFound("User not found");

            return Ok(await userRepository.GetUserById(id));
        }
        catch (Exception)
        {
            return NotFound("Error getting user.");
        }
    }

    [HttpGet("producers"), Authorize(Roles = "Owner")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetProducers()
    {
        try
        {
            return Ok(await userRepository.GetProducers());
        }
        catch (Exception)
        {
            return NotFound("Error getting producers.");
        }
    }
}