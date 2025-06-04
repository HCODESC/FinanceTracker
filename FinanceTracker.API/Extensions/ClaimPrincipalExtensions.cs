using System.Security.Claims;

namespace FinanceTracker.API.Extensions;

public static class ClaimPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst("userId") ?? user.FindFirst(ClaimTypes.NameIdentifier);  
        return claim != null && Guid.TryParse(claim.Value, out var userId) ? userId: Guid.Empty;
    }
}