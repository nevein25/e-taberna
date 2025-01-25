using System.Security.Claims;

namespace Order.Application.Extentions;

public static class ClaimsPrincipalExtensions
{
    public static int GetLoggedInUserId(this ClaimsPrincipal user)
    {
        return int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId) ? userId : throw new InvalidOperationException("UserId is missing in the claims."); ;
    }

    public static string GetLoggedInUserRole(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Role)?.Value ?? throw new InvalidOperationException("Role is missing in the claims."); ;
    }
}
