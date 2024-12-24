using ProductCatalog.API.Extentions;
using ProductCatalog.API.Seeders;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.RegisterServices(builder.Configuration, Assembly.GetExecutingAssembly());
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());



var app = builder.Build();
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
