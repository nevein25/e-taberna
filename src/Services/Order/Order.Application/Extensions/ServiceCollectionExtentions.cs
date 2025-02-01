using BuildingBlocks.Messaging.Configurations;
using BuildingBlocks.Messaging.MessageBuses;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Order.Application.Extensions;
public static class ServiceCollectionExtentions
{
    public static void AddApplication(this IServiceCollection services, Assembly assembly, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(assembly);
        TypeAdapterConfig.GlobalSettings.Scan(assembly);


        services.Configure<RabbitMQConfigurations>(configuration.GetSection(nameof(RabbitMQConfigurations)));

        services.AddSingleton<IMessageBus, RabbitMQMessageBus>();
    }

}
