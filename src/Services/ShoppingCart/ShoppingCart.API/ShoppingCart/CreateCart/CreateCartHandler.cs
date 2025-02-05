using BuildingBlocks.Messaging.MessageBuses;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Exceptions;
using ShoppingCart.API.Extentions;
using ShoppingCart.API.Models;
using ShoppingCart.API.Presestance;
using ShoppingCart.API.ShoppingCart.ProductService;

namespace ShoppingCart.API.ShoppingCart.CreateCart;


public record CreateCartCommand(List<CreateCartItem> CartItems, int UserId) : IRequest<CreateCartResult>;
public record CreateCartItem(int ProductId, int Quantity);
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


        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.");
    }
}

internal class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductApiService _productService;
    private readonly IMessageBus _messageBus;

    //private readonly IValidator<CreateCartCommand> _validator;

    public CreateCartHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor, IProductApiService productService, IMessageBus messageBus
                /* , IValidator<CreateCartCommand> validator*/)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _productService = productService;
        _messageBus = messageBus;
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
        int? userId = _httpContextAccessor.GetLoggedInUserId();

        var userCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (userCart is not null) return new CreateCartResult(userCart.Id);

        var mappedCart = command.Adapt<Cart>();

        if (userId is not null) mappedCart.UserId = (int)userId;



        var productIdsToAdd = command.CartItems.Select(item => item.ProductId).ToList();

        var productsResponse = await _productService.GetProductsAsync(productIdsToAdd) ?? throw new ProductServiceUnavailableException();
        var cartItems = new List<CartItem>();
        foreach (var cartItem in command.CartItems)
        {

            var product = productsResponse.Products.FirstOrDefault(p => p.Id == cartItem.ProductId);
            if (product is null) throw new ProductNotFoundException(cartItem.ProductId);
            if (product.Quantity < cartItem.Quantity) throw new InsufficientQuantityException(cartItem.ProductId, cartItem.Quantity, product.Quantity);


            var item = new CartItem
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                ProductName = product.Name,
                Price = product.Price
            };
            cartItems.Add(item);
        }
        mappedCart.CartItems = cartItems;
        _context.Add(mappedCart);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateCartResult(mappedCart.Id);
    }
}
