using System.ComponentModel.DataAnnotations; using FinanceTracker.API.Model; 
namespace FinanceTracker.API.DTOs.Transaction;

public class TransactionRequestDto
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")] public decimal Amount { get; set; }
    [Required] public TransactionType Type { get; set; }
    [MaxLength(500)] public string Note { get; set; }
    [Required] public DateTime TransactionDate { get; set; }
    [Required] public Guid CategoryId { get; set; } 
}