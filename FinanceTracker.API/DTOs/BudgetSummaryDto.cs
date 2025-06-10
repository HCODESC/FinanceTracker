namespace FinanceTracker.API.DTOs.Budget
{
    public class BudgetSummaryDto
    {
            public Guid Id { get; set; }
            public String CategoryName { get; set; }
            public decimal LimitAmount { get; set; }
            public decimal AmountSpent { get; set; }
            public decimal RemainingAmount => LimitAmount - AmountSpent;
            public double PercentageUsed => LimitAmount == 0 ? 0 : (double)(AmountSpent/ LimitAmount) * 100; 
            public bool IsOverBudget => AmountSpent > LimitAmount;

    }
}
