using FinanceTracker.API.Services.UserProfile;
using FinanceTracker.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class UserProfileController(IUserProfileService service, ILogger<UserProfileController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileRequestDto userProfileRequestDto)
    {
        try
        {
            var supabaseId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            if (supabaseId == string.Empty)
            {
                return Unauthorized();
            }

            var result = await service.CreateUserProfileDtoAsync(userProfileRequestDto, supabaseId);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(new { error = result.ErrorMessage });
        } catch (Exception ex)
        {
            logger.LogError("An error occurred while processing the request.\n" + ex.Message);
            return StatusCode(500, new { error = "An error occurred while processing the request." });
        }
    }
}
