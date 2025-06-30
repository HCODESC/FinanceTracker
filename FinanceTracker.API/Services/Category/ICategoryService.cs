using FinanceTracker.Shared.DTOs;
using FinanceTracker.API.Helpers;

namespace FinanceTracker.API.Services.Category;

public interface ICategoryService
{
   Task<ServiceResult<CategoryResponseDto>> CreateCategory(CategoryRequestDto categoryRequestDto, Guid userId);
   Task<ServiceResult<CategoryResponseDto>> GetCategoryByIdAsync(Guid categoryId, Guid userId);
   Task<ServiceResult<CategoryResponseDto>> GetCategoryByNameAsync(String categoryName, Guid userId);
   Task<ServiceResult<List<CategoryResponseDto>>> GetAllCategoriesAsync(Guid userId );
   Task<ServiceResult<CategoryResponseDto>> EditCategoryAsync(CategoryRequestDto categoryRequestDto, Guid userId, Guid categoryId);
   Task<ServiceResult<bool>> DeleteCategoryAsync(Guid id, Guid userId);
}