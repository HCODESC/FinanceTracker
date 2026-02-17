namespace FinanceTracker.API.Model
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string SupabaseId { get; set; }
        public string? ProfileImgUrl { get; set; } = String.Empty; 
        public string Username { get; set; }
        public string Email { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Budget> Budgets { get; set; }

    }
}
