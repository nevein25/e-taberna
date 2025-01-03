using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Extentions;
using ShoppingCart.API.Presestance;
using ShoppingCart.API.ShoppingCart.UpdateCart;

namespace ShoppingCart.API.ShoppingCart.DeleteCart;


public record DeleteCartCommand() : IRequest<DeleteCartResult>;
public record DeleteCartResult(bool IsSuccess);

public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResult>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteCartHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<DeleteCartResult> Handle(DeleteCartCommand command, CancellationToken cancellationToken)
    {
        try
        {

            int? userId = _httpContextAccessor.GetLoggedInUserId();
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null) return new DeleteCartResult(false);

            _context.Remove(cart);
            await _context.SaveChangesAsync();
            return new DeleteCartResult(true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
