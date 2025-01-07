
using Carter;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.API.Extensions;
using ShoppingCart.API.Extentions;
using ShoppingCart.API.Seeders;
using ShoppingCart.Exceptions.Handler;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.RegisterServices(builder.Configuration, Assembly.GetExecutingAssembly());
builder.AddJwtAuthentication();
builder.Services.AddHttpContextAccessor();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();


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

app.UseExceptionHandler();

var scoped = app.Services.CreateScope();
var seeder = scoped.ServiceProvider.GetRequiredService<ISeeder>();
await seeder.SeedAsync();

// using IExceptionHandler is better
//// for  returning a structured Json response containing the error details, which is more readable and informative.
//app.UseExceptionHandler(exceptionHandlerApp =>
//{
//    exceptionHandlerApp.Run(async context =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        if (exception is null) return;

//        var problemDetails = new ProblemDetails
//        {
//            Title = exception.Message,
//            Status = StatusCodes.Status500InternalServerError,
//            Detail = exception.StackTrace
//        };

//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/problem+json";


//        await context.Response.WriteAsJsonAsync(problemDetails);
//    });
//});
app.Run();
