using FinanceTracker.API.Data;
using FinanceTracker.API.Helpers;
using FinanceTracker.Shared.DTOs;
using FinanceTracker.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services.Report;

public class ReportService(FinanceTrackerDbContext context, ILogger<ReportService>? logger) : IReportService
{
    public async Task<ServiceResult<MonthlyReportDto>> GetMonthlySummaryAsync(Guid userId, int month, int year)
    {
        try
        {
            var transactions = await context.Transactions.Where(t => t.UserProfileId == userId && t.TransactionDate.Month == month && t.TransactionDate.Year == year).ToListAsync();

            var totalExpense = transactions
                .Where(t => t.Type == TransactionType.EXPENSE)
                .Sum(t => t.Amount);
            
            var totalIncome = transactions
                .Where(t => t.Type == TransactionType.INCOME)
                .Sum(t => t.Amount);
            var summary = new MonthlyReportDto()
            {
                Month = month,
                Year = year,
                TotalIncome = totalIncome, 
                TotalExpenses = totalExpense 
            };

         
            return ServiceResult<MonthlyReportDto>.Success(summary);
        }
        catch (Exception e)
        {
            logger.LogError(e, "There was an issue generating the monthly report");
           return ServiceResult<MonthlyReportDto>.Failure(e.Message);
        } 
    }
}