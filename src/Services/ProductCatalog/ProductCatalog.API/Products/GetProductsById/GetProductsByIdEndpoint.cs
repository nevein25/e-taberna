using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;

namespace ProductCatalog.API.Products.GetProductsById;

public record GetProductByIdResponse(int Id, string Name, string Description, string ImageFile, decimal Price, string CategoryName, int Quantity);

public record GetProductsByIdResponse(List<GetProductByIdResponse> Products);


public class GetProductsByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products", async (int[] ids, AppDbContext context) =>
        {
            var products = await context.Products
                .Include(p => p.Category)
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            if (!products.Any()) return Results.NotFound();

            var productResponses = products.Select(product => product.Adapt<GetProductByIdResponse>()).ToList();

            return Results.Ok(new GetProductsByIdResponse(productResponses));
        })
        .WithTags(nameof(Product))
        .ProducesValidationProblem(StatusCodes.Status404NotFound)
        .Produces<GetProductsByIdResponse>(StatusCodes.Status200OK);
    }

}
