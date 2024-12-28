
using Carter;
using ShoppingCart.API.Extensions;
using ShoppingCart.API.Extentions;
using ShoppingCart.API.Seeders;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.RegisterServices(builder.Configuration, Assembly.GetExecutingAssembly());
builder.AddJwtAuthentication();
builder.Services.AddHttpContextAccessor();



var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();//  maps the routes defined in the ICarterModule implementation.
                //  scans code and looking for the ICarterModule implementation and map the required Http methods.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

    var scoped = app.Services.CreateScope();
    var seeder = scoped.ServiceProvider.GetRequiredService<ISeeder>();
    await seeder.SeedAsync();



app.Run();
