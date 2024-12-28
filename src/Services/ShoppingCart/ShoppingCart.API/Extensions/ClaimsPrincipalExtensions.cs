using System.Security.Claims;

namespace ShoppingCart.API.Extentions;

public static class HttpContextAccessorExtensions
{
    public static int? GetLoggedInUserId(this IHttpContextAccessor HttpContextAccessor)
    {
        return int.TryParse(HttpContextAccessor?.HttpContext?.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, out int userId) ? userId : null;
    }

    public static string? GetLoggedInUserRole(this IHttpContextAccessor HttpContextAccessor)
    {
       return HttpContextAccessor?.HttpContext?.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
    }
}
