using BuildingBlocks.Messaging.Events;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Interfaces;
using ProductCatalog.API.Persistance;

namespace ProductCatalog.API.Products.AdjustInventoryOnOrderPaid;

public class AdjustInventoryHandler
{
    private readonly IAppDbContext _context;

    public AdjustInventoryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(OrderPaidEvent orderPaidEvent)
    {

        foreach (var paidProduct in orderPaidEvent.Products)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == paidProduct.Id);
            if (product is not null)
                product.Quantity -= paidProduct.Quantity;
           
        }

        await _context.SaveChangesAsync();
    }
}