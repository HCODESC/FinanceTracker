using AutoMapper;
using FinanceTracker.API.Data;
using FinanceTracker.Shared.DTOs;
using FinanceTracker.API.Helpers;
using FinanceTracker.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services.Budget;

public class BudgetService(ILogger<BudgetService> logger, FinanceTrackerDbContext context, IMapper mapper): IBudgetService
{
    public async Task<ServiceResult<BudgetRequestDto>> CreateOrUpdateBudget(BudgetRequestDto budgetRequestDto, Guid userId)
    {
        try
        {
            var existing = await context.Budgets.FirstOrDefaultAsync(
                b => b.CategoryId == budgetRequestDto.CategoryId && 
                     b.UserId == userId &&
                     b.Month == budgetRequestDto.Month &&
                     b.Year == budgetRequestDto.Year);

            if (existing is null)
            {
                
                var newBudget = mapper.Map<Model.Budget>(budgetRequestDto);
                newBudget.Id= Guid.NewGuid();
                newBudget.CategoryId = budgetRequestDto.CategoryId;
                newBudget.UserId = userId;
                await context.Budgets.AddAsync(newBudget);
                await context.SaveChangesAsync();
                
                var response = mapper.Map<BudgetRequestDto>(newBudget);
                return ServiceResult<BudgetRequestDto>.Success(response);
            }

            existing.LimitAmount = budgetRequestDto.LimitAmount;
            context.Budgets.Update(existing); 
            await context.SaveChangesAsync();
            return ServiceResult<BudgetRequestDto>.Success(mapper.Map<BudgetRequestDto>(existing));
        }
        catch (Exception e)
        {
            logger.LogError(e, "There was an error creating budget for user: {UserId}", userId);
            return ServiceResult<BudgetRequestDto>.Failure("There was an error creating budget"); 
        } 
    }

    public async Task<ServiceResult<List<BudgetSummaryDto>>> GetBudgetSummary(Guid userId, int month, int year)
    {
        try
        {

            var summary = await context.Budgets
                .Include(b => b.Category)
                .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
                .Select(b => new BudgetSummaryDto
                {
                    Id = b.Id,
                    CategoryName = b.Category.Name, 
                    LimitAmount = b.LimitAmount,
                    AmountSpent = context.Transactions.Where(t => t.UserId == userId 
                                                                  && t.CategoryId == b.CategoryId 
                                                                  && t.TransactionDate.Month == month 
                                                                  && t.TransactionDate.Year == year
                                                                  && t.Type == TransactionType.EXPENSE)
                        .Sum(t => (decimal?)t.Amount) ?? 0
                }).ToListAsync();
        
            return ServiceResult<List<BudgetSummaryDto>>.Success(summary);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while retrieving budget summaries for user {UserId}", userId);
            return ServiceResult<List<BudgetSummaryDto>>.Failure("There was an error getting budget summaries");
        }
    }

    public async Task<ServiceResult<bool>> DeleteBudget(Guid budgetId, Guid userId)
    {
        try
        {
            var budget = await context.Budgets.FindAsync(budgetId);
            
            if (budget == null)
                return ServiceResult<bool>.Failure("Budget not found");
            
            if(budget.UserId != userId)
                return ServiceResult<bool>.Failure("You are not authorized to delete this budget");
            
            context.Budgets.Remove(budget);
            await context.SaveChangesAsync();
            return ServiceResult<bool>.Success(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "There was an error deleting budget: {UserId}", userId);
            return ServiceResult<bool>.Failure("There was an error deleting budget");
        }
    }
}