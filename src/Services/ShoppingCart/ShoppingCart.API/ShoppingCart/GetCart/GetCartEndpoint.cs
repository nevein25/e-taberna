using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ShoppingCart.API.Constants;
using ShoppingCart.API.Extentions;

namespace ShoppingCart.API.ShoppingCart.GetCart;


public record GetCartResponse(List<GetCartItemResponse> CartItems);
public record GetCartItemResponse(int ProductId, string ProductName, decimal Price, int Quantity);
public class GetCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/carts", [Authorize(Roles = Roles.Customer)] async (ISender sender, IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = httpContextAccessor.GetLoggedInUserId();
            if (userId is null) return Results.BadRequest();

            var result = await sender.Send(new GetCartQuery((int)userId));

            var response = result.Adapt<GetCartResponse>();

            return Results.Ok(response);
        })
        .WithName("GetCart")
        .Produces<GetCartItemResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("GetCart")
        .WithDescription("GetCart");
    }
}
