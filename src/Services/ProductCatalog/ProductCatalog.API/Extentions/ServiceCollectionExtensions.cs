using BuildingBlocks.Messaging.Configurations;
using BuildingBlocks.Messaging.MessageBuses;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.ApplicationSettings;
using ProductCatalog.API.Interfaces;
using ProductCatalog.API.Persistance;
using ProductCatalog.API.Products.AdjustInventoryOnOrderPaid;
using ProductCatalog.API.Products.CreateProduct;
using ProductCatalog.API.Products.DeleteProduct;
using ProductCatalog.API.Products.GetProductById;
using ProductCatalog.API.Products.GetProductsById;
using ProductCatalog.API.Products.UpdateProduct;
using ProductCatalog.API.Seeders;
using System.Reflection;
namespace ProductCatalog.API.Extentions;
public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
    {

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ProductCatalogDb")));
        services.AddScoped<IAppDbContext>( sp => sp.GetRequiredService<AppDbContext>());

        services.AddValidatorsFromAssembly(assembly)
         .AddFluentValidationAutoValidation();

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());


        services.AddScoped<ISeeder, Seeder>();

        services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));

        services.AddAuthentication();
        services.AddAuthorization();

        services.AddScoped<AdjustInventoryHandler>();
        services.AddHostedService<OrderPaidEventConsumer>();


        services.Configure<RabbitMQConfigurations>(configuration.GetSection(nameof(RabbitMQConfigurations)));

        services.AddSingleton<IMessageBus, RabbitMQMessageBus>();
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<DeleteProductHandler>();
        services.AddScoped<GetProductByIdHandler>();
        services.AddScoped<GetProductsByIdHandler>();



    }
}

