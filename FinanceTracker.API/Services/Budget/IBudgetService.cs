using FinanceTracker.API.DTOs.Budget;
using FinanceTracker.API.Helpers;

namespace FinanceTracker.API.Services.Budget;

public interface IBudgetService
{
  Task<ServiceResult<BudgetRequestDto>> CreateOrUpdateBudget(BudgetRequestDto budgetRequestDto, Guid userId); 
  Task<ServiceResult<List<BudgetSummaryDto>>> GetBudgetSummary(Guid userId, int month, int year);
  Task<ServiceResult<bool>> DeleteBudget(Guid budgetId,Guid userId);
}