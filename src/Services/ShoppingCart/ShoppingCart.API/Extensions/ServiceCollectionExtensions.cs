using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Presestance;

namespace ShoppingCart.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Micro.CartDb")));

    }
}
