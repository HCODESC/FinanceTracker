namespace FinanceTracker.Shared.DTOs;

public class AuthResponseDto
{
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}
