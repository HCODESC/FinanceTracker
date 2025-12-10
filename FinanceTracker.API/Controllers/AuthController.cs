using FinanceTracker.Shared.DTOs;
using FinanceTracker.API.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(IAuthService authService) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var (isSuccess, errorMessage) = await authService.RegisterAsync(registerDto);
        
        if (!isSuccess)
        {
            return BadRequest(new { message = errorMessage });
        }

        return Ok(new { message = "User registered successfully" });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var (isSuccess, response, errorMessage) = await authService.LoginAsync(loginDto);
        
        if (!isSuccess)
        {
            return Unauthorized(new { message = errorMessage });
        }

        return Ok(response);
    }
    
    [HttpGet("test")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult Test()
    {
        var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        return Ok(new { 
            message = "JWT is working!", 
            username = username,
            userId = userId 
        });
    }
}