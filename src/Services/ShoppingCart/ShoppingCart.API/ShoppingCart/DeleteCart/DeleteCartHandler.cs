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
    private readonly ICartDeletionService _cartDeletionService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteCartHandler(ICartDeletionService cartDeletionService, IHttpContextAccessor httpContextAccessor)
    {
        _cartDeletionService = cartDeletionService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<DeleteCartResult> Handle(DeleteCartCommand command, CancellationToken cancellationToken)
    {
        try
        {
            int? userId = _httpContextAccessor.GetLoggedInUserId();
            if(userId is null) return new DeleteCartResult(false);

            return await _cartDeletionService.DeleteCartAsync((int)userId, cancellationToken);

        }
        catch
        {
            throw;
        }
    }
}
