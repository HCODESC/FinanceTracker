using System.ComponentModel.DataAnnotations;
using FinanceTracker.Shared.Enums;

namespace FinanceTracker.Shared.DTOs;

public class TransactionRequestDto
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")] public decimal Amount { get; set; }
    [Required] public TransactionType Type { get; set; }
    [MaxLength(500)] public string Note { get; set; }
    [Required] public DateTime TransactionDate { get; set; }
    public Guid? CategoryId { get; set; } 
    [MaxLength(50)]
    public string? CreateCategoryName { get; set; } 
}