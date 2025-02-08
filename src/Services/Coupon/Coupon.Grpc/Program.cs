using Coupon.Grpc.Extensions;
using Coupon.Grpc.Services;
using Coupon.DataAccess.Extensions;
using Coupon.Business.Extensions;
using Coupon.Business.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;
builder.Services.AddGrpc();
builder.Services.AddPresentation();
builder.Services.AddJwtAuthentication(configuration);
builder.Services.AddDataAccessServices(configuration);
builder.Services.AddBusiness(configuration);

var app = builder.Build();
await app.UseMigrationsAsync();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<CouponGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
