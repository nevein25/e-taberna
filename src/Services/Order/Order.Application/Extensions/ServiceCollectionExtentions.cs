using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Order.Application.Extensions;
public static class ServiceCollectionExtentions
{
    public static void AddApplication(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
        TypeAdapterConfig.GlobalSettings.Scan(assembly);


    }

}
