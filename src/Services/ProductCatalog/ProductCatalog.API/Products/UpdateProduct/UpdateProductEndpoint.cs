using Azure.Core;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Constants;
using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Extentions;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;
using System.Security.Claims;

namespace ProductCatalog.API.Products.UpdateProduct;

public record UpdateProductRequest(string Name, string Description, string ImageFile, decimal Price, string CategoryName);
public record UpdateProductResponse(int Id);

public class UpdateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/products/{id}", [Authorize(Roles = Roles.Seller)] async (int id, UpdateProductRequest updateProductRequest,
                                                 IValidator<UpdateProductRequest> validator,
                                                ClaimsPrincipal user, UpdateProductHandler handler) =>
        {
            var validationResult = await validator.ValidateAsync(updateProductRequest);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

            int? userId = user.GetLoggedInUserId();
            if (userId is null) return Results.BadRequest();

            var (response, isSuccess) = await handler.Handle(id, updateProductRequest, (int)userId);

            if (isSuccess)
                return Results.Ok(response);

            return Results.NotFound();

        })
        .WithTags(nameof(Product))
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }


}
