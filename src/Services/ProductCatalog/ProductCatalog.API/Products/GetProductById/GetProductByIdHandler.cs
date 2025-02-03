using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Interfaces;

namespace ProductCatalog.API.Products.GetProductById;

public class GetProductByIdHandler
{
    private readonly IAppDbContext _context;

    public GetProductByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetProductByIdResponse?> Handle(int productId)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == productId);

        return product.Adapt<GetProductByIdResponse>();
    }
}
