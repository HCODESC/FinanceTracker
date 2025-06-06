namespace FinanceTracker.API.DTOs.Budget
{
    public class BudgetRequestDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal LimitAmount { get; set; }
        public Guid CategoryId { get; set; }
    }
}
