using FinanceTracker.API.DTOs;
using FinanceTracker.API.Helpers;

namespace FinanceTracker.API.Services.Report;

public interface IReportService
{
   Task<ServiceResult<MonthlyReportDto>> GetMonthlySummaryAsync(Guid userId, int month, int year); 
}