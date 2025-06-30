using FinanceTracker.Shared.Enums;
namespace FinanceTracker.API.Model
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string Note { get; set; } = string.Empty;
        
        public DateTime TransactionDate { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; 
        
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
