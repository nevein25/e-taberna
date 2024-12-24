using System.Security.Claims;

namespace ProductCatalog.API.Extentions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetLoggedInUserId(this ClaimsPrincipal user)
    {
        return int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId) ? userId : null;
    }

    public static string? GetLoggedInUserRole(this ClaimsPrincipal user)
    {
        return user?.FindFirst(ClaimTypes.Role)?.Value;
    }
}
