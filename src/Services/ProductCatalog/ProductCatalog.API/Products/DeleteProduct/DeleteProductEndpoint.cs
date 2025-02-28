using Microsoft.AspNetCore.Authorization;
using ProductCatalog.API.Constants;
using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Extentions;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;
using System.Security.Claims;

namespace ProductCatalog.API.Products.DeleteProduct;


public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", [Authorize(Roles = Roles.Seller)] async (int id, DeleteProductHandler handler, ClaimsPrincipal user) =>
        {

            int? userId = user.GetLoggedInUserId();
            if (userId is null)
                return Results.BadRequest();

            var response = await handler.Handle(id, (int)userId);

            return response.IsSuccess
                ? Results.Ok(response)
                : Results.NotFound(response);
        })
         .RequireAuthorization()
         .ProducesProblem(StatusCodes.Status404NotFound)
         .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
         .WithTags(nameof(Product));
    }
}
