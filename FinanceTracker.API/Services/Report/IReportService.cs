using FinanceTracker.Shared;
using FinanceTracker.API.Helpers;
using FinanceTracker.Shared.DTOs;

namespace FinanceTracker.API.Services.Report;

public interface IReportService
{
   Task<ServiceResult<MonthlyReportDto>> GetMonthlySummaryAsync(Guid userId, int month, int year); 
}