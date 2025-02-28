using Mapster;
using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.SharedKernel.CQRS;
using Order.SharedKernel.Results;

namespace Order.Application.Orders.Commands;
public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    private IAppDbContext _context;

    public CreateOrderHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CreateOrderResult>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateOrderCommandValidator().ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)        
            return Result.Failure<CreateOrderResult>(Error.Problem("ValidationError", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))));
        

        if (command.Order is null)
            return Result.Failure<CreateOrderResult>(Error.NullValue);


        var order = command.Order.Adapt<Domain.Models.Order>();


        var productIds = order.OrderItems.Select(oi => oi.Product.Id).ToList();

        var existingProducts = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        foreach (var orderItem in order.OrderItems)
        {
            if (existingProducts.TryGetValue(orderItem.Product.Id, out var existingProduct))
            {
                existingProduct.Price = orderItem.Product.Price;
                existingProduct.Quantity = orderItem.Product.Quantity;
                existingProduct.Name = orderItem.Product.Name;

                orderItem.Product = existingProduct;
            }
            else
            {
                _context.Products.Add(orderItem.Product);
            }
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(new CreateOrderResult(order.Id));

    }
}
