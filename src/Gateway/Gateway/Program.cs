using Gateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddJwtAuthentication();

var envName = builder.Environment.EnvironmentName;
var configFile = envName.Equals("Production", StringComparison.OrdinalIgnoreCase)
    ? "ocelot.Production.json"
    : "ocelot.json";

builder.Configuration.AddJsonFile(configFile, optional: false, reloadOnChange: true);

builder.Services.AddOcelot (builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


await app.UseOcelot();

app.Run();
