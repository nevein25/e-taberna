using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Extentions;
using ShoppingCart.API.Models;
using ShoppingCart.API.Presestance;

namespace ShoppingCart.API.ShoppingCart.UpdateCart;



public record UpdateCartCommand(List<UpdateCartItem> CartItems) : IRequest<UpdateCartResult>;
public record UpdateCartItem(int ProductId, string ProductName, decimal Price, int Quantity);

public record UpdateCartResult(bool IsSuccessful);
public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateCartHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<UpdateCartResult> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
    {

        int? userId = _httpContextAccessor.GetLoggedInUserId();

        //TODO: check product existence, handle quantity
        var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null) return new UpdateCartResult(false);

        var existingItems = cart.CartItems.ToList();
        var updatedItems = command.CartItems;

        foreach (var updatedItem in updatedItems)
        {
            var existingItem = existingItems
                .FirstOrDefault(e => e.ProductId == updatedItem.ProductId);

            if (existingItem != null)
            {
                existingItem.ProductName = updatedItem.ProductName;
                existingItem.Price = updatedItem.Price;
                existingItem.Quantity = updatedItem.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = updatedItem.ProductId,
                    ProductName = updatedItem.ProductName,
                    Price = updatedItem.Price,
                    Quantity = updatedItem.Quantity
                });
            }
        }

        foreach (var existingItem in existingItems)
            if (!updatedItems.Any(u => u.ProductId == existingItem.ProductId))
                _context.CartItems.Remove(existingItem);


        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateCartResult(true);
    }
}
