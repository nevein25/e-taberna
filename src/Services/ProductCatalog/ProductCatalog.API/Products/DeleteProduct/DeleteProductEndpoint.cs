using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;

namespace ProductCatalog.API.Products.DeleteProduct;


public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/products/{id}", async (int id, AppDbContext context) =>
        {
            var product = context.Products.FirstOrDefault(p => p.Id == id);
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
         .ProducesProblem(StatusCodes.Status404NotFound)
         .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
         .WithTags(nameof(Product));
    }
}
