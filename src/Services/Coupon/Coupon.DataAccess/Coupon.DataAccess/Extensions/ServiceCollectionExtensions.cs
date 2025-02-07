using Coupon.DataAccess.Persistence;
using Coupon.DataAccess.Presistance;
using Coupon.DataAccess.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coupon.DataAccess.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IAppDbContext, AppDbContext>(opt =>
                                        opt.UseSqlServer(configuration.GetConnectionString("CoupunDb")));

        services.AddScoped<ISeeder, Seeder>();

    }
}
