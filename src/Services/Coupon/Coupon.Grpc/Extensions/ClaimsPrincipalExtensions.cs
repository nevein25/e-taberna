using System.Security.Claims;

namespace Coupon.Grpc.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetLoggedInUserId(this ClaimsPrincipal user)
    {
        var userIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdStr, out int userId) ? userId : null;
    }
}
