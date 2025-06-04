using FinanceTracker.API.Model;

namespace FinanceTracker.API.DTOs.Category
{
    public class CategoryResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
