using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;

namespace ProductCatalog.API.Products.GetProductsById;

public record GetProductById(int Id, string Name, string Description, string ImageFile, decimal Price, string CategoryName, int Quantity);

public record GetProductsByIdResponse(List<GetProductById> Products);


public class GetProductsByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products", async (int[] ids, GetProductsByIdHandler handler) =>
        {
            var response = await handler.Handle(ids);
            return response?.Products.Any() == true
                ? Results.Ok(response)
                : Results.NotFound();
        })
        .WithTags(nameof(Product))
        .ProducesValidationProblem(StatusCodes.Status404NotFound)
        .Produces<GetProductsByIdResponse>(StatusCodes.Status200OK);
    }

}
