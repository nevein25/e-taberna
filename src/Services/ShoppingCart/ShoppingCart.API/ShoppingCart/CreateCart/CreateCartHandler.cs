// Ignore Spelling: Validator

using FluentValidation;
using Mapster;
using MediatR;
using ShoppingCart.API.Extentions;
using ShoppingCart.API.Models;
using ShoppingCart.API.Presestance;
using System.Security.Claims;

namespace ShoppingCart.API.ShoppingCart.CreateCart;


public record CreateCartCommand(List<CreateCartItem> CartItems, int UserId) : IRequest<CreateCartResult>;
public record CreateCartItem(int ProductId, string ProductName, decimal Price, int Quantity);
public record CreateCartResult(int CartId);

public class CreateCartRequestValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartRequestValidator()
    {
        RuleForEach(x => x.CartItems).SetValidator(new CreateCartItemValidator());

    }
}
public class CreateCartItemValidator : AbstractValidator<CreateCartItem>
{
    public CreateCartItemValidator()
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

internal class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    //private readonly IValidator<CreateCartCommand> _validator;

    public CreateCartHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor
                /* , IValidator<CreateCartCommand> validator*/)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        //_validator = validator;
    }

    public async Task<CreateCartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        /* no need after using ValidationBehavior
        var result = await _validator.ValidateAsync(command, cancellationToken);
        var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
        if (errors.Any())
        {
            throw new ValidationException(errors.FirstOrDefault());
        }*/

        var mappedCart = command.Adapt<Cart>();
        int? userId = _httpContextAccessor.GetLoggedInUserId();

        if (userId is not null)
            mappedCart.UserId = (int)userId;

        //TODO: check product existence, handle quantity
        _context.Add(mappedCart);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateCartResult(mappedCart.Id);
    }
}
