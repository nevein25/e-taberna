using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Interfaces;
using Order.Application.Payments.Interfaces;
using Order.Infrastructure.Payments;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Settings;
using Stripe;
using Stripe.Checkout;

namespace Order.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("OrderDb")));
        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddAuthentication();
        services.AddAuthorization();

        var stripeSection = config.GetSection("StripeConfiguration");
        services.Configure<StripeAppConfigration>(stripeSection);
        var tokenCon = stripeSection.Get<StripeAppConfigration>() ?? throw new Exception("Can not find StripeConfiguration in the appsettings");
    
        StripeConfiguration.ApiKey = tokenCon.SecretKey;
        services.AddScoped<IPaymentService, StripeService>();
        services.AddSingleton<SessionService>();
        services.AddSingleton<PaymentIntentService>();

        services.AddScoped<IPaymentStatusMapper, PaymentStatusMapper>();

    }
}
