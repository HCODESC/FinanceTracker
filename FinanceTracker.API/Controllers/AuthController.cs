using FinanceTracker.API.DTOs.Auth;
using FinanceTracker.API.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(IAuthService authService, IConfiguration configuration) : ControllerBase
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
        var token = await authService.LoginAsync(loginDto);
        
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var jwtSettings = configuration.GetSection("JwtSettings");
        var expirationMinutes = Convert.ToDouble(jwtSettings["ExpiresInMinutes"]);

        return Ok(new
        {
            token = token,
            expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
        });
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