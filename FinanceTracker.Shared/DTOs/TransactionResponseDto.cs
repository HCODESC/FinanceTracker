
using FinanceTracker.Shared.Enums;

namespace FinanceTracker.Shared.DTOs
{
    public class TransactionResponseDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string Note { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
