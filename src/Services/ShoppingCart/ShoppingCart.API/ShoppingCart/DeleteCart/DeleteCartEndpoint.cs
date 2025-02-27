using Carter;
using ShoppingCart.API.Constants;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Mapster;

namespace ShoppingCart.API.ShoppingCart.DeleteCart;

public record DeleteCartResponse(bool IsSuccess);

public class DeleteCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/carts", [Authorize(Roles = Roles.Customer)] async (ISender sender) =>
        {
            var result = await sender.Send(new DeleteCartCommand());

            var response= result.Adapt<DeleteCartResponse>();

            return Results.Ok(response);

        }).WithName("DeleteCart")
        .Produces<DeleteCartResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Cart")
        .WithDescription("Delete Cart"); ;
    }
}
