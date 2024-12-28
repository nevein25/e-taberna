
using ShoppingCart.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();



app.MapCarter();//  maps the routes defined in the ICarterModule implementation.
                //  scans code and looking for the ICarterModule implementation and map the required Http methods.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
