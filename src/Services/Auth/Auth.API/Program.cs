using Auth.Infrastructure.Extentions;
using Auth.Infrastructure.Seeders;
namespace Auth.API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();


        //var seeder = app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeeder>();
        //await seeder.SeedAsync();
        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
        await seeder.SeedAsync();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();


        app.Run();
    }
}
