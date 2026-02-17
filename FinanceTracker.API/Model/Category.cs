namespace FinanceTracker.API.Model
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Budget> Budgets { get; set; }
    }
}
