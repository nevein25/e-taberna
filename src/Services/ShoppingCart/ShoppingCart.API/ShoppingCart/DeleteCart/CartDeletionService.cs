using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Extentions;
using ShoppingCart.API.Presestance;

namespace ShoppingCart.API.ShoppingCart.DeleteCart;

public interface ICartDeletionService
{
    Task<DeleteCartResult> DeleteCartAsync(int customerId, CancellationToken cancellationToken);
}

public class CartDeletionService : ICartDeletionService
{
    private readonly AppDbContext _context;

    public CartDeletionService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<DeleteCartResult> DeleteCartAsync(int cusomerId, CancellationToken cancellationToken)
    {

        var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == cusomerId, cancellationToken);
        if (cart is null) return new DeleteCartResult(false);

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync(cancellationToken);
        return new DeleteCartResult(true);
    }
}
