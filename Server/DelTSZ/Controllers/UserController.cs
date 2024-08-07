﻿using DelTSZ.Models.Users;
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
                return NotFound(new { message = "User not found." });

            var user = await userRepository.GetUserAllDataById(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error getting user." });
        }
    }

    [HttpGet("producers"), Authorize(Roles = "Owner")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetProducers()
    {
        try
        {
            return Ok(await userRepository.GetProducerUserResponses());
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error getting producers." });
        }
    }

    [HttpGet("customers"), Authorize(Roles = "Owner")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetCustomers()
    {
        try
        {
            return Ok(await userRepository.GetCustomerUserResponses());
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Error getting costumers." });
        }
    }
}