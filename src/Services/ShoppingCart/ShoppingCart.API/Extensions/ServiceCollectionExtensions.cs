using BuildingBlocks.Messaging.Configurations;
using BuildingBlocks.Messaging.MessageBuses;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.ApplicationSettings;
using ShoppingCart.API.Behaviors;
using ShoppingCart.API.Presestance;
using ShoppingCart.API.Seeders;
using ShoppingCart.API.ShoppingCart.DeleteCart;
using ShoppingCart.API.ShoppingCart.ProductService;
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
            opt.AddOpenBehavior(typeof(LoggingBehavior<,>));

        });
        services.AddValidatorsFromAssembly(assembly);
        services.AddCarter();



        services.AddScoped<ISeeder, Seeder>();
        services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));
        services.AddAuthentication();
        services.AddAuthorization();

        var productUrl = configuration["ServiceUrls:ProductCatalogAPI"] ?? throw new Exception("Can not ProductCatalogAPI find in the appsettings");
    
        services.AddHttpClient<IProductApiService, ProductApiService>(client =>
        {
            client.BaseAddress = new Uri(productUrl);
        });

        services.Configure<RabbitMQConfigurations>(configuration.GetSection(nameof(RabbitMQConfigurations)));

        services.AddSingleton<IMessageBus, RabbitMQMessageBus>();

        services.AddHostedService<DeleteCartOnOrderPaidConsumer>();

        services.AddScoped<ICartDeletionService, CartDeletionService>();    
    }
}
