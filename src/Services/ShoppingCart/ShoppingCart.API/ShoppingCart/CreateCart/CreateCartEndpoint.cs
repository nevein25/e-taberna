using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ShoppingCart.API.Constants;
using System.Security.Claims;

namespace ShoppingCart.API.ShoppingCart.CreateCart;


public record CreateCartRequest(List<CreateCartItemRequest> CartItems, int UserId);
public record CreateCartItemRequest(int ProductId, string ProductName, decimal Price, int Quantity);

public record CreateCartResponse(int CartId);

public class CreateCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/carts", [Authorize(Roles = Roles.Customer)] async (ISender sender, CreateCartRequest cart, ClaimsPrincipal user) =>
        {
            CreateCartCommand createCartCommand = cart.Adapt<CreateCartCommand>();

            CreateCartResult result = await sender.Send(createCartCommand);

            CreateCartResponse response = result.Adapt<CreateCartResponse>();

            return Results.Created($"cart/{response.CartId}", response);
        })
        .WithName("CreateCart")
        .Produces<CreateCartResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Cart")
        .WithDescription("Create Cart");
    }
}
