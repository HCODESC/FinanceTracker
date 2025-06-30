using FinanceTracker.Shared.DTOs;
using FinanceTracker.API.Extensions;
using FinanceTracker.API.Services.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class TransactionController(ITransactionService service, ILogger<TransactionController> logger): ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionRequestDto  transactionRequestDto)
    {
        try
        {
            // Get user ID from JWT token/claims
            var userId = User.GetUserId(); 
            
            if (userId == Guid.Empty)
                return Unauthorized();

            // Validate the request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.CreateTransactionAsync(transactionRequestDto, userId);

            if (result.IsSuccess)
            {
               return Ok(result.Data); 
            }

            return BadRequest(new { error = result.ErrorMessage });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in CreateTransaction endpoint");
            return StatusCode(500, new { error = "An unexpected error occurred" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var userId = User.GetUserId();

        if (userId == Guid.Empty)
            return Unauthorized();

        var response = await service.GetAllTransactionsAsync(userId,  page, pageSize);

        if (response.TotalCount <= 0)
        {
            return NotFound();
        } 
        
        return Ok(response);
    }
    
    [HttpGet("{transactionId}")]
    public async Task<IActionResult> GetTransactionById(Guid transactionId)
    {
        try
        {
           var userId = User.GetUserId();
           if(userId == Guid.Empty)
               return Unauthorized();
            
           var result = await service.GetTransactionByIdAsync(transactionId, userId);

           if (!result.IsSuccess)
           {
               return NotFound(new { error = result.ErrorMessage });
           }
            
           return Ok(result.Data);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error in GetTransactionById endpoint");
            return StatusCode(500, new { error = "An unexpected error occurred" });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTransaction([FromBody] TransactionRequestDto transactionRequestDto, Guid transactionId)
    {
        try
        {
            
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await service.EditTransactionAsync(transactionRequestDto, transactionId, userId);

            return !result.IsSuccess ? StatusCode(500,new { error = result.ErrorMessage }) : Ok(result.Data);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error in UpdateTransaction endpoint");
            return StatusCode(500, new { error = "An unexpected error occurred" });
        } 
    }

    [HttpDelete("{transactionId}")]
    public async Task<IActionResult> DeleteTransaction(Guid transactionId)
    {
        var userId = User.GetUserId(); 
        if (userId == Guid.Empty)
            return Unauthorized();
        
        var deleted= await service.DeleteTransactionAsync(transactionId, userId);
        return deleted ? NoContent() : NotFound();
    }
}