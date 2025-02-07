
using Coupon.DataAccess.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace Coupon.Grpc.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task<IApplicationBuilder> UseMigrationsAsync(this IApplicationBuilder app)
    {

        IServiceScope scope = app.ApplicationServices.CreateScope();
        var seedService = scope.ServiceProvider.GetService<ISeeder>();
        if (seedService is not null) await seedService.SeedAsync();

        return app;
    }
}
