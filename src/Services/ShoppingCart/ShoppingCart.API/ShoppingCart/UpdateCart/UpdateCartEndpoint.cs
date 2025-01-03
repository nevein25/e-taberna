using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ShoppingCart.API.Constants;
using ShoppingCart.API.ShoppingCart.CreateCart;

namespace ShoppingCart.API.ShoppingCart.UpdateCart;


public record UpdateCartRequest(List<UpdateCartItemRequest> CartItems);
public record UpdateCartItemRequest(int ProductId, string ProductName, decimal Price, int Quantity);

public record UpdateCartResponse(bool IsSuccessful);

public class UpdateCartRequestValidator : AbstractValidator<UpdateCartCommand>
{
    public UpdateCartRequestValidator()
    {
        RuleForEach(x => x.CartItems).SetValidator(new UpdateCartItemValidator());

    }
}
public class UpdateCartItemValidator : AbstractValidator<UpdateCartItem>
{
    public UpdateCartItemValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0.");

        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithMessage("ProductName is required.")
            .MaximumLength(100)
            .WithMessage("ProductName must not exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.");
    }
}


public class UpdateCartEndpoint : ICarterModule
{

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/carts", [Authorize(Roles = Roles.Customer)] async (ISender sender, UpdateCartRequest cart) =>
        {
            UpdateCartCommand updateCartCommand = cart.Adapt<UpdateCartCommand>();

            UpdateCartResult result = await sender.Send(updateCartCommand);

            UpdateCartResponse response = result.Adapt<UpdateCartResponse>();

            return Results.Ok(response);
        })
        .WithName("UpdateCart")
        .Produces<UpdateCartResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Cart")
        .WithDescription("Update Cart");
    }
}
