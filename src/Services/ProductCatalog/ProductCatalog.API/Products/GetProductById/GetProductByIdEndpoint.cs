using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;

namespace ProductCatalog.API.Products.GetProductById;

public record GetProductByIdResponse(string Name, string Description, string ImageFile, decimal Price, string CategoryName, int Quantity);


public class GetProductByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products/{id}", async (int id, AppDbContext context) =>
        {
            var product = await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return Results.NotFound();

            var productResponse = product.Adapt<GetProductByIdResponse>();
            return Results.Ok(productResponse);


        }).WithTags(nameof(Product))
        .ProducesValidationProblem(StatusCodes.Status404NotFound)
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK);
    }
}
