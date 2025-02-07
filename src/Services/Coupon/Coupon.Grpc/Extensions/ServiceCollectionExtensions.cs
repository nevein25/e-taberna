using Coupon.Grpc.Persistence;
using Coupon.Grpc.Presistance;
using Coupon.Grpc.Seeders;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IAppDbContext, AppDbContext>(opt => 
                                        opt.UseSqlServer(configuration.GetConnectionString("CoupunDb")));

        services.AddScoped<ISeeder, Seeder>();

        services.AddAuthentication();
        services.AddAuthorization();
    }
}
