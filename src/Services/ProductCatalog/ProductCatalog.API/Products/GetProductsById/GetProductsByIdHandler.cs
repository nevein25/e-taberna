using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Interfaces;

namespace ProductCatalog.API.Products.GetProductsById;

public class GetProductsByIdHandler
{
    private readonly IAppDbContext _context;

    public GetProductsByIdHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<GetProductsByIdResponse?> Handle(int[] productIds)
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        if (!products.Any())
            return null;

        var productResponses = products.Select(product => product.Adapt<GetProductByIdResponse>()).ToList();
        return new GetProductsByIdResponse(productResponses);
    }
}

