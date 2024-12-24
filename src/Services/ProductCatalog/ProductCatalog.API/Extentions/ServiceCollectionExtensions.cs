using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Persistance;
using ProductCatalog.API.Seeders;
using System.Reflection;
namespace ProductCatalog.API.Extentions;
public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configration, Assembly assembly)
    {

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configration.GetConnectionString("ProductCatalogDb")));
     
        services.AddValidatorsFromAssembly(assembly)
         .AddFluentValidationAutoValidation();

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());


        services.AddScoped<ISeeder, Seeder>();

    }
}

