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
        app.MapGet("/products/{id}", async (int id, GetProductByIdHandler handler) =>
        {
            var response = await handler.Handle(id);
            return response is null
                ? Results.NotFound()
                : Results.Ok(response);


        }).WithTags(nameof(Product))
        .ProducesValidationProblem(StatusCodes.Status404NotFound)
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK);
    }
}
