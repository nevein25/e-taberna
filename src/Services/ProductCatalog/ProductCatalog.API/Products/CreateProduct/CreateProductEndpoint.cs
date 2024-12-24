using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Constants;
using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Extentions;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;
using System.Security.Claims;

namespace ProductCatalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, string Description, string ImageFile, decimal Price, string CategoryName, int SellerId);
public record CreateProductResponse(int Id);


public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(p => p.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(p => p.CategoryName).NotEmpty().WithMessage("Category is required");
        RuleFor(p => p.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}


public class CreateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/products", [Authorize(Roles = Roles.Seller)] async (CreateProductRequest createProduct, AppDbContext context,
                                            IValidator<CreateProductRequest> validator, ClaimsPrincipal user) =>
        {
            var validationResult = await validator.ValidateAsync(createProduct);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

            var userId = user.GetLoggedInUserId();
            if (userId == null) Results.BadRequest();

            var category = await context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == createProduct.CategoryName.ToLower());
            if (category is null)
            {
                category = new Category
                {
                    Name = createProduct.CategoryName.ToLower()
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
            }

            var mappedProduct = createProduct.Adapt<Product>();
            mappedProduct.Category = category;
            mappedProduct.SellerId = (int)userId!;

            await context.AddAsync(mappedProduct);
            await context.SaveChangesAsync();

            return Results.Created($"/products/{mappedProduct.Id}", mappedProduct);
        }).RequireAuthorization()
          .WithTags(nameof(Product));

    }
}
