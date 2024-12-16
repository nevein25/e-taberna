using Auth.Domain.Model;
using Auth.Infrastructure.Persistance;
using Auth.Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure.Extentions;
public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configration)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configration.GetConnectionString("AuthDb")));

        services.AddIdentityCore<User>()
                 .AddRoles<Role>()
                 .AddRoleManager<RoleManager<Role>>()
                 .AddEntityFrameworkStores<AppDbContext>();

        services.AddScoped<ISeeder, Seeder>();
    }
}

