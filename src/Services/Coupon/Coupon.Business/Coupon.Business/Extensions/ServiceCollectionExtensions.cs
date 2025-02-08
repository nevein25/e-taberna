using Coupon.Business.Services;
using Coupon.Business.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

namespace Coupon.Business.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddBusiness(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICouponService, Services.CouponService>();
        services.AddScoped<IStripeService, StripeService>();


        var stripeSection = configuration.GetSection("StripeConfiguration");
        services.Configure<StripeAppConfiguration>(stripeSection);
        var tokenCon = stripeSection.Get<StripeAppConfiguration>() ?? throw new Exception("Can not find StripeConfiguration in the appsettings");

        StripeConfiguration.ApiKey = tokenCon.SecretKey;
    }
}
