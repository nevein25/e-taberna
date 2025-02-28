using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Constants;
using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Extentions;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;
using ProductCatalog.API.Products.UpdateProduct;
using System.Security.Claims;

namespace ProductCatalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, string Description, string ImageFile, decimal Price, string CategoryName, int Quantity, int SellerId);
public record CreateProductResponse(int Id);


public class CreateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", [Authorize(Roles = Roles.Seller)] async (CreateProductRequest createProduct, CreateProductHandler handler,
                                            IValidator<CreateProductRequest> validator, ClaimsPrincipal user) =>
        {
            var validationResult = await validator.ValidateAsync(createProduct);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

            int? userId = user.GetLoggedInUserId();
            if (userId is null) return Results.BadRequest();

            var (response, isSuccess) = await handler.Handle(createProduct, (int)userId);

            if (response is null || !isSuccess) return Results.BadRequest();


            return Results.Created($"/products/{response.Id}", response);


        }).RequireAuthorization()
          .WithTags(nameof(Product));

    }
}
