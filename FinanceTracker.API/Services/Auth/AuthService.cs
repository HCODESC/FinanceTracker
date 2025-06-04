using FinanceTracker.API.Data;
using FinanceTracker.API.DTOs.Auth;
using FinanceTracker.API.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services.Auth;

public class AuthService(FinanceTrackerDbContext context, ITokenService tokenService)
    : IAuthService
{
    public async Task<(bool isSuccess, string errorMessage)> RegisterAsync(RegisterDto registerDto)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(registerDto.Username) || 
            string.IsNullOrWhiteSpace(registerDto.Email) || 
            string.IsNullOrWhiteSpace(registerDto.Password))
        {
            return (false, "Username, email, and password are required");
        }

        // Check if email already exists
        if (await context.Users.AnyAsync(u => u.Email == registerDto.Email))
            return (false, "Email already exists");

        // Check if username already exists
        if (await context.Users.AnyAsync(u => u.Username == registerDto.Username))
            return (false, "Username already exists");

        // Basic email validation
        if (!IsValidEmail(registerDto.Email))
            return (false, "Invalid email format");

        // Basic password validation
        if (registerDto.Password.Length < 6)
            return (false, "Password must be at least 6 characters long");

        try
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                CreatedAt = DateTime.UtcNow
            };
            
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync(); 

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            // Log the exception if you have logging configured
            return (false, "An error occurred during registration");
        }
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(loginDto.Username) || 
                string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return string.Empty; // Return empty string for invalid input
            }

            // Find user by username or email
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            // Check if user exists and password is correct
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return string.Empty; // Return empty string for invalid credentials
            }

            // Generate and return JWT token
            return tokenService.GenerateToken(user.Id, user.Username);
        }
        catch (Exception ex)
        {
            // Log the exception if you have logging configured
            return string.Empty;
        }
    }
    
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    } 
}