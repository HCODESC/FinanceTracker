using FinanceTracker.API.Data;
using FinanceTracker.API.Helpers;
using FinanceTracker.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services.UserProfile
{
    public class UserProfileService(FinanceTrackerDbContext context, ILogger<UserProfileService> logger) : IUserProfileService
    {
        public async Task<ServiceResult<UserProfileRequestDto>> CreateUserProfileDtoAsync(UserProfileRequestDto userProfileRequestDto, string supabaseId)
        {
            var existingProfile = await context.UserProfiles.FirstOrDefaultAsync(up => up.SupabaseId == supabaseId);
            try
            {
                // Check if a user profile with the same SupabaseId already exists
                if (existingProfile != null)
                {
                    logger.LogInformation("User profile already exists for SupabaseId: {SupabaseId}", supabaseId);
                    return ServiceResult<UserProfileRequestDto>.Failure("User profile already exists.");
                }

                var userProfile = new Model.UserProfile
                {
                    Id = Guid.NewGuid(),
                    SupabaseId = supabaseId,
                    Username = userProfileRequestDto.Username,
                    Email = userProfileRequestDto.Email
                }; 
                await context.UserProfiles.AddAsync(userProfile);
                await context.SaveChangesAsync();
                logger.LogInformation("User profile created successfully for SupabaseId: {SupabaseId}", supabaseId);
                return ServiceResult<UserProfileRequestDto>.Success(userProfileRequestDto);
            }
            catch (Exception ex)
            { 
                logger.LogError(ex, "Error creating user profile for SupabaseId: {SupabaseId}", supabaseId);
                return ServiceResult<UserProfileRequestDto>.Failure($"Error creating user profile: {ex.Message}");
            }
        }

        public async Task<ServiceResult<UserProfileResponseDto>> GetUserProfileAsync(string supabaseId)
        {
            var userProfile = await context.UserProfiles.FindAsync(supabaseId);

            if (userProfile == null)
            {
                logger.LogWarning("User profile not found for SupabaseId: {SupabaseId}", supabaseId);
                return ServiceResult<UserProfileResponseDto>.Failure("User profile not found.");
            }

            return new ServiceResult<UserProfileResponseDto>
            {
                IsSuccess = true,
                Data = new UserProfileResponseDto
                {
                    ProfileImgUrl = userProfile.ProfileImgUrl,
                    Username = userProfile.Username,
                    Email = userProfile.Email
                }
            };
        }
    }
}
