using FinanceTracker.API.Extensions;
using FinanceTracker.API.Services.Budget;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.Shared.DTOs;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class BudgetController(IBudgetService service, ILogger<BudgetController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateBudget([FromBody] BudgetRequestDto budgetRequestDto)
    {
        try
        {
            var userId = User.GetUserId();
        
            if(userId == Guid.Empty)
                return Unauthorized();
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await service.CreateOrUpdateBudget(budgetRequestDto, userId);

            if (result.IsSuccess)
                return Ok(result.Data); 
            
            return BadRequest(new { result.ErrorMessage });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating budget");
            return StatusCode(500, new { error = "An unexpected error occured" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetBudgetSummaries(
       [FromQuery] int month,
       [FromQuery] int year 
        )
    {
        try
        {
            var userId = User.GetUserId();

            if (userId == Guid.Empty)
                return Unauthorized(); 

            var result = await service.GetBudgetSummary(userId, month, year);
            
            if(result.IsSuccess)
                return Ok(result.Data);
            
            return BadRequest(new { result.ErrorMessage });
            
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting budget summaries");
            return StatusCode(500, new { error = "An unexpected error occured" });
        } 
    }

    [HttpDelete("{budgetId}")]
    public async Task<IActionResult> DeleteBudget(Guid budgetId)
    {
        try
        {
            var userId = User.GetUserId();
       
            if(userId == Guid.Empty) 
                return Unauthorized();
       
            var result = await service.DeleteBudget(budgetId, userId);
       
            if (result.IsSuccess)
                return Ok(result.Data);
       
            return BadRequest(new { result.ErrorMessage });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting budget");
            return StatusCode(500, new { error = "An unexpected error occured" });
        } 
    }
}