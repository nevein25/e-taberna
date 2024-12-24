using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Constants;
using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Extentions;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ProductCatalog.API.Products.UpdateProduct;



public record UpdateProductRequest(string Name, string Description, string ImageFile, decimal Price, string CategoryName);
public record UpdateProductResponse(int Id);


public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(p => p.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(p => p.CategoryName).NotEmpty().WithMessage("Category is required");
        RuleFor(p => p.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public class UpdateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/products/{id}", [Authorize(Roles = Roles.Seller)] async (int id, UpdateProductRequest updateProductRequest,
                                                AppDbContext context, IValidator<UpdateProductRequest> validator,
                                                ClaimsPrincipal user) =>
        {
            var validationResult = await validator.ValidateAsync(updateProductRequest);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (user.GetLoggedInUserId() != product?.SellerId)
                return Results.BadRequest();

            if (product is null) return Results.NotFound();

            var category = await context.Categories.FirstOrDefaultAsync(c => c.Name == updateProductRequest.CategoryName);

            if (category is null)
            {
                category = new Category { Name = updateProductRequest.CategoryName.ToLower() };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
            }

            product.Name = updateProductRequest.Name;
            product.Description = updateProductRequest.Description;
            product.ImageFile = updateProductRequest.ImageFile;
            product.Price = updateProductRequest.Price;
            product.Category = category;

            await context.SaveChangesAsync();

            var response = new UpdateProductResponse(product.Id);
            return Results.Ok(response);

        })
        .WithTags(nameof(Product))
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }


}
