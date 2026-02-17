using AutoMapper;

namespace FinanceTracker.API.Model
{
    public class Budget
    {
        public Guid Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal LimitAmount { get; set; }

        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
