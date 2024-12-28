using Mapster;
using MediatR;
using ShoppingCart.API.Extentions;
using ShoppingCart.API.Models;
using ShoppingCart.API.Presestance;
using System.Security.Claims;

namespace ShoppingCart.API.ShoppingCart.CreateCart;


public record CreateCartCommand(List<CreateCartItem> CartItems, int UserId) : IRequest<CreatCartResult>;
public record CreateCartItem(int ProductId, string ProductName, decimal Price, int Quantity);
public record CreatCartResult(int CartId);

internal class CreateCartHandler : IRequestHandler<CreateCartCommand, CreatCartResult>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateCartHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CreatCartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var mappedCart = command.Adapt<Cart>();
        int? userId = _httpContextAccessor.GetLoggedInUserId();

        if (userId is not null)
            mappedCart.UserId = (int)userId;

        //TODO: check product existance, handle quantity
        _context.Add(mappedCart);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreatCartResult(mappedCart.Id);
    }
}
