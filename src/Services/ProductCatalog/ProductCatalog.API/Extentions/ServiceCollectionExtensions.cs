using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Persistance;
using ProductCatalog.API.Seeders;

namespace ProductCatalog.API.Extentions;
public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configration)
    {

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configration.GetConnectionString("ProductCatalogDb")));

        services.AddScoped<ISeeder, Seeder>();

    }
}

