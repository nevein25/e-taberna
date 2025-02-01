using Order.Application.Extensions;
using Order.Infrastructure.Extensions;
using Order.SharedKernel.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = builder.Configuration;
builder.Services.AddInfrastructure(config);
builder.Services.AddApplication(Assembly.GetExecutingAssembly(), config);
builder.Services.AddCQRS(typeof(Order.Application.Orders.Commands.CreateOrderCommand).Assembly);
builder.AddJwtAuthentication();




var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
