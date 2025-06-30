using AutoMapper;
using FinanceTracker.API.Data;
using FinanceTracker.API.Helpers;
using FinanceTracker.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services.Category;

public class CategoryService(FinanceTrackerDbContext context, IMapper mapper, ILogger<CategoryService> logger)
    : ICategoryService
{
    public async Task<ServiceResult<CategoryResponseDto>> CreateCategory(CategoryRequestDto categoryRequestDto, Guid userId)
    {
        try
        {
            var categoryExists = await context.Categories.AnyAsync(c => c.Name == categoryRequestDto.Name && c.UserId == userId);
        
            if(categoryExists)
                return ServiceResult<CategoryResponseDto>.Failure("Category already exists");
            
            var newCategory = mapper.Map<Model.Category>(categoryRequestDto);
            newCategory.Id = Guid.NewGuid();
            newCategory.UserId = userId;
            
            await context.Categories.AddAsync(newCategory);
            await context.SaveChangesAsync();
            
            var categoryResponseDto = mapper.Map<CategoryResponseDto>(newCategory);
            return ServiceResult<CategoryResponseDto>.Success(categoryResponseDto);

        }
        catch (Exception e)
        {
            logger.LogError(e, "There was an error creating category for user: {UserId}", userId);
            return ServiceResult<CategoryResponseDto>.Failure("There was an error creating category");
        } 
    }

    public async Task<ServiceResult<CategoryResponseDto>> GetCategoryByIdAsync(
        Guid categoryId,
        Guid userId
    )
    {
        try
        {
            var category = await context.Categories.FindAsync(categoryId);

            if (category != null && category.UserId != userId)
                return ServiceResult<CategoryResponseDto>.Failure(
                    "You are not authorized to view this category"
                );

            if (category == null)
                return ServiceResult<CategoryResponseDto>.Failure("The category does not exists");

            var responseDto = mapper.Map<CategoryResponseDto>(category);
            return ServiceResult<CategoryResponseDto>.Success(responseDto);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "There was an error getting category {CategoryId} for user {UserId}",
                categoryId,
                userId
            );
            return ServiceResult<CategoryResponseDto>.Failure(
                "There was an error getting category"
            );
        }
    }

    public async Task<ServiceResult<CategoryResponseDto>> GetCategoryByNameAsync(
        string categoryName,
        Guid userId
    )
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(c =>
                c.Name == categoryName && c.UserId == userId
            );

            if (category == null)
                return ServiceResult<CategoryResponseDto>.Failure("Category not found");

            var responseDto = mapper.Map<CategoryResponseDto>(category);
            return ServiceResult<CategoryResponseDto>.Success(responseDto);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An error occured getting category by name {CategoryName}",
                categoryName
            );
            return ServiceResult<CategoryResponseDto>.Failure(
                "An error occured getting category by name"
            );
        }
    }

    public async Task<ServiceResult<List<CategoryResponseDto>>> GetAllCategoriesAsync(Guid userId)
    {
        try
        {
            var categories = await context.Categories.Where(c => c.UserId == userId).ToListAsync(); 
            
            var response = categories
                .Select(c => mapper.Map<CategoryResponseDto>(c))
                .ToList();
            
            return ServiceResult<List<CategoryResponseDto>>.Success(response);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failure to get categories for {UserId}", userId);
            return ServiceResult<List<CategoryResponseDto>>
                .Failure("There was an error getting all categories"); 
        }
    }


    public async Task<ServiceResult<CategoryResponseDto>> EditCategoryAsync(
        CategoryRequestDto categoryRequestDto,
        Guid categoryId,
        Guid userId 
    )
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(c =>
                c.Id == categoryId 
            );

            if (category == null)
                return ServiceResult<CategoryResponseDto>.Failure(
                    "Category not found"
                );
            
            if(category.UserId != userId)
                return ServiceResult<CategoryResponseDto>.Failure("You are not authorized to edit this category");

            var nameExists = await context.Categories.AnyAsync(c =>
                c.Name == category.Name && c.UserId == userId && c.Id != categoryId
            );

            if (nameExists)
            {
                return ServiceResult<CategoryResponseDto>.Failure(
                    "Category with that name already exists"
                );
            }

            category.Name = categoryRequestDto.Name;
            context.Categories.Update(category);
            await context.SaveChangesAsync();

            var responseDto = mapper.Map<CategoryResponseDto>(category);
            return ServiceResult<CategoryResponseDto>.Success(responseDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error happened while editing category");
            return ServiceResult<CategoryResponseDto>.Failure(
                "There was an error editing category"
            );
        }
    }

    public async Task<ServiceResult<bool>> DeleteCategoryAsync(Guid id, Guid userId)
    {
        try
        {
            var category = await context.Categories.FindAsync(id);

            if (category == null)
                return ServiceResult<bool>.Failure("Category not found");

            if (category.UserId != userId)
                return ServiceResult<bool>.Failure("You are not authorized to view this category");
            
            var hasTransactions = await context.Transactions.AnyAsync(t => t.CategoryId == id);
            if (hasTransactions)
                return ServiceResult<bool>.Failure("Cannot delete category with existing transactions.");


            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "There was an error deleting category for user {UserId}", userId);
            return ServiceResult<bool>.Failure("There was an error deleting category");
        }
    }
}
