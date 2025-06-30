
using FinanceTracker.Shared.DTOs;

namespace FinanceTracker.API.Services.Auth;

public interface IAuthService
{
    Task<(bool isSuccess, string errorMessage)> RegisterAsync(RegisterDto registerDto); 
    Task<string> LoginAsync(LoginDto loginDto);
}