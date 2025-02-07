namespace Coupon.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddAuthentication();
        services.AddAuthorization();
    }
}
