namespace FinanceTracker.Shared.DTOs;

public class YearlyReportDto
{
   public int Year { get; set; }
   public decimal TotalIncome { get; set; }
   public decimal TotalExpense { get; set; }
   public decimal NetBalance => TotalIncome - TotalExpense; 
   
   List<MonthlyReportDto> MonthlyReports { get; set; } = new();
}