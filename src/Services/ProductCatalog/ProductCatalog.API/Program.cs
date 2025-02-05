using ProductCatalog.API.Extentions;
using ProductCatalog.API.Seeders;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var assembly = Assembly.GetExecutingAssembly();

builder.Services.RegisterServices(builder.Configuration, assembly);
builder.Services.AddEndpoints(assembly);
builder.AddJwtAuthentication();


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

IServiceScope scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
await seeder.SeedAsync();


app.Run();
