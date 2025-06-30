namespace FinanceTracker.Shared.DTOs;

public class MonthlyReportDto
{
    public int Month { get; set; }
    public int? Year { get; set; }
    public decimal TotalIncome { get; set; } 
    public decimal TotalExpenses { get; set; }
    public decimal NetBlanace => TotalIncome - TotalExpenses;
}