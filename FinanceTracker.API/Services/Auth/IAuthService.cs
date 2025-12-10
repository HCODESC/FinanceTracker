
using FinanceTracker.Shared.DTOs;

namespace FinanceTracker.API.Services.Auth;

public interface IAuthService
{
    Task<(bool isSuccess, string errorMessage)> RegisterAsync(RegisterDto registerDto); 
    Task<(bool isSuccess, AuthResponseDto? response, string errorMessage)> LoginAsync(LoginDto loginDto);
}