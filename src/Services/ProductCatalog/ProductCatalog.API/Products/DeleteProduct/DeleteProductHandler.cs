using ProductCatalog.API.Interfaces;

namespace ProductCatalog.API.Products.DeleteProduct;

public class DeleteProductHandler
{
    private readonly IAppDbContext _context;

    public DeleteProductHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteProductResponse> Handle(int productId, int userId)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == productId);

        if (userId != product?.SellerId || product is null)
            return new DeleteProductResponse(false);


        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return new DeleteProductResponse(true);

    }
}
