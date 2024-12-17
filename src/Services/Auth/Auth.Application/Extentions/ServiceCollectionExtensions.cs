
using Auth.Application.ApplicationSettings;
using Auth.Application.CreateUserFactory;
using Auth.Application.Interfaces;
using Auth.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Auth.Application.Extentions;
public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<UserFactoryResolver>();

        services.AddScoped<IUserFactory, CustomerFactory>();
        services.AddScoped<IUserFactory, SellerFactory>();
    }
}

