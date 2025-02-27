using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ShoppingCart.API.Constants;
using ShoppingCart.API.Extentions;
using ShoppingCart.API.ShoppingCart.CreateCart;
using System.Security.Claims;

namespace ShoppingCart.API.ShoppingCart.ApplyCoupon;

public record ApplyCouponRequest(int CouponId);
public record ApplyCouponResponse(bool Success);

public class ApplyCouponEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/carts/coupon", [Authorize(Roles = Roles.Customer)] async (ISender sender, ApplyCouponRequest applyCouponRequest, IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = httpContextAccessor.GetLoggedInUserId();
            if (userId is null) return Results.Unauthorized();
            
            ApplyCouponCommand command = new ApplyCouponCommand(applyCouponRequest.CouponId, (int)userId);

            var result = await sender.Send(command);

            var response = result.Adapt<ApplyCouponResponse>();

            return Results.Created($"/api/carts/coupon", response);
        })
        .WithName("Apply Coupon")
        .Produces<ApplyCouponResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Apply Coupon")
        .WithDescription("Apply Coupon"); 
    }
}
