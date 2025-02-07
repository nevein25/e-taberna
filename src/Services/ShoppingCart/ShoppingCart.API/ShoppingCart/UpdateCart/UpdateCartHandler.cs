using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Exceptions;
using ShoppingCart.API.Extentions;
using ShoppingCart.API.Models;
using ShoppingCart.API.Persistance;
using ShoppingCart.API.Presestance;
using ShoppingCart.API.ShoppingCart.ProductService;

namespace ShoppingCart.API.ShoppingCart.UpdateCart;



public record UpdateCartCommand(List<UpdateCartItem> CartItems) : IRequest<UpdateCartResult>;
public record UpdateCartItem(int ProductId, int Quantity);

public record UpdateCartResult(bool IsSuccessful);
public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
{
    private readonly IAppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductQueryService _productService;

    public UpdateCartHandler(IAppDbContext context, IHttpContextAccessor httpContextAccessor, IProductQueryService productService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _productService = productService;
    }
    public async Task<UpdateCartResult> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
    {
        int? userId = _httpContextAccessor.GetLoggedInUserId();

        var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null) return new UpdateCartResult(false);

        var existingItems = cart.CartItems.ToList();
        var updatedItems = command.CartItems;


        var productIds = updatedItems.Select(item => item.ProductId).ToList();
        var productsResponse = await _productService.GetProductsAsync(productIds) ?? throw new ProductServiceUnavailableException();

        foreach (var cartItem in updatedItems)
        {
            var product = productsResponse.Products.FirstOrDefault(p => p.Id == cartItem.ProductId);

            if (product is null) throw new ProductNotFoundException(cartItem.ProductId);


            if (product.Quantity < cartItem.Quantity) throw new InsufficientQuantityException(cartItem.ProductId, cartItem.Quantity, product.Quantity);


            var existingItem = existingItems
                 .FirstOrDefault(e => e.ProductId == cartItem.ProductId);

            if (existingItem is not null)
                existingItem.Quantity = cartItem.Quantity;

            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = cartItem.ProductId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = cartItem.Quantity
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
