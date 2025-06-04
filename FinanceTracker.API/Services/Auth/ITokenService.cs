namespace FinanceTracker.API.Services.Auth;

public interface ITokenService
{
    string GenerateToken(Guid userId, string username);
}