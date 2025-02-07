using Coupon.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Coupon.Business.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddBusiness(this IServiceCollection services)
    {
        services.AddScoped<ICouponService, CouponService>();

    }
}
