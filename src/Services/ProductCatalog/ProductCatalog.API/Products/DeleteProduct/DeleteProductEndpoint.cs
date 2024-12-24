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
        app.MapDelete("api/products/{id}", [Authorize(Roles = Roles.Seller)] async (int id, AppDbContext context, ClaimsPrincipal user) =>
        {
            var product = context.Products.FirstOrDefault(p => p.Id == id);

            if (user.GetLoggedInUserId() != product?.SellerId)
                return Results.BadRequest();

            if (product is null)
            {
                var response = new DeleteProductResponse(false);
                return Results.NotFound(response);
            }
            else
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                var response = new DeleteProductResponse(true);

                return Results.Ok(response);
            }
        })
         .RequireAuthorization()
         .ProducesProblem(StatusCodes.Status404NotFound)
         .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
         .WithTags(nameof(Product));
    }
}
