using AutoMapper;
using FinanceTracker.API.DTOs.Category;
using FinanceTracker.API.Extensions;
using FinanceTracker.API.Services.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]/[action]")]
public class CategoryController(ICategoryService service, IMapper mapper, ILogger<CategoryController> logger)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDto categoryRequestDto)
    {
        try
        {
            var userId = User.GetUserId();

            if (userId == Guid.Empty)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.CreateCategory(categoryRequestDto, userId);

            if (result.IsSuccess) return Ok(result.Data);

            return BadRequest(new { result.ErrorMessage });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error in CreateCategory Endpoint");
            return StatusCode(500, new { error = "An unexpected error occured" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var userId = User.GetUserId();

            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await service.GetAllCategoriesAsync(userId); 
            
            if(result.IsSuccess) return Ok(result.Data);
            
            return BadRequest(new { result.ErrorMessage });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error in GetCategories Endpoint");
            return StatusCode(500, new { error = "An unexpected error occured" });
        }
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategoryById(Guid categoryId)
    {
        try
        {
            var userId = User.GetUserId();
            
            if(userId == Guid.Empty)
                return Unauthorized();

            var result = await service.GetCategoryByIdAsync(categoryId, userId); 
            
            if(result.IsSuccess) return Ok(result.Data);
            
            return BadRequest(new { result.ErrorMessage });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error in GetCategoryById Endpoint");    
            return StatusCode(500, new { error = "An unexpected error occured" });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequestDto categoryRequestDto, Guid categoryId)
    {
        try
        {
            var userId = User.GetUserId();
       
            if (userId == Guid.Empty)
                return Unauthorized();
        
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
        
            var result = await service.EditCategoryAsync(categoryRequestDto, categoryId ,userId);
            
            if (result.IsSuccess) return Ok(result.Data);
            
            return BadRequest(new { result.ErrorMessage });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error in UpdateCategory Endpoint");
            return StatusCode(500, new { error = "An unexpected error occured" });
        } 
    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteCategory(Guid categoryId)
    {
        try
        {
            var userId = User.GetUserId();
        
            if(userId == Guid.Empty) 
                return Unauthorized();
        
            var result = await service.DeleteCategoryAsync(categoryId, userId);
        
            if(result.IsSuccess) return Ok(result.Data);
        
            return BadRequest(new { result.ErrorMessage });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error in DeleteCategory Endpoint");
            return StatusCode(500, new { error = "An unexpected error occured" });
        } 
       
    }
}