namespace FinanceTracker.API.Model
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string SupabaseId { get; set; }
        public string? ProfileImgUrl { get; set; } = String.Empty; 
        public string Username { get; set; }
        public string Email { get; set; }

    }
}
