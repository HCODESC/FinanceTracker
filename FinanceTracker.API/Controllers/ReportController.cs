using FinanceTracker.API.Extensions;
using FinanceTracker.API.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class ReportController(IReportService service, ILogger<ReportController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetReport([FromQuery] int month, [FromQuery] int year)
    {
        try
        {
            var userId = User.GetUserId();

            if (userId == Guid.Empty) return Unauthorized();

            var result = await service.GetMonthlySummaryAsync(userId, month, year);
            if (result.IsSuccess)  return Ok(result);
            
            return BadRequest(new { error = result.ErrorMessage});
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in ReportController endpoint");
            return StatusCode(500, new { error = "An unexpected error occurred" });
        }
    }
}