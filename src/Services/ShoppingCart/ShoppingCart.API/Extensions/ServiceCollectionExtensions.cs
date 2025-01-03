using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.ApplicationSettings;
using ShoppingCart.API.Behaviors;
using ShoppingCart.API.Presestance;
using ShoppingCart.API.Seeders;
using System.Reflection;

namespace ShoppingCart.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("CartDb")));


        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(assembly);
            opt.AddOpenBehavior(typeof(ValidationBehavior<,>));

        });
        services.AddValidatorsFromAssembly(assembly);
        services.AddCarter();



        services.AddScoped<ISeeder, Seeder>();
        services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));
        services.AddAuthentication();
        services.AddAuthorization();
    }
}
