using Microsoft.Extensions.DependencyInjection;
using Order.SharedKernel.CQRS;
using Order.SharedKernel.Messaging;
using System.Reflection;

namespace Order.SharedKernel.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCQRS(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<ISender, Sender>();
        var handlers = assembly
                      .GetTypes()
                      .Where(t => !t.IsAbstract && !t.IsInterface) 
                      .SelectMany(t => t.GetInterfaces(), (type, interfaceType) => new { type, interfaceType })
                      .Where(ti => ti.interfaceType.IsGenericType &&
                                   ti.interfaceType.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

        foreach (var handler in handlers)
        {
            services.AddScoped(handler.interfaceType, handler.type);
        }

        return services;
    }
}
