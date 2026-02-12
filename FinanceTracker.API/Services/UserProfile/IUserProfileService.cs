using FinanceTracker.API.Helpers;
using FinanceTracker.Shared.DTOs;
namespace FinanceTracker.API.Services.UserProfile
{
    public interface IUserProfileService
    {
        Task<ServiceResult<UserProfileResponseDto>> GetUserProfileAsync(string supabaseId);
        Task<ServiceResult<UserProfileRequestDto>> CreateUserProfileDtoAsync(UserProfileRequestDto userProfileRequestDto, string supabaseId);
    }
}
