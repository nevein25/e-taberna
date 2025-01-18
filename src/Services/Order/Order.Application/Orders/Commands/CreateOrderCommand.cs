using FluentValidation;
using Order.Application.Orders.DTOs;
using Order.Domain.Models;
using Order.SharedKernel.CQRS;

namespace Order.Application.Orders.Commands;
public record CreateOrderCommand(OrderRequest Order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(int Id);


public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.Address).NotEmpty().WithMessage("Name is required");

        RuleForEach(x => x.Order.OrderItems)
             .SetValidator(new OrderItemValidator());
    }
}




public class ProductValidator : AbstractValidator<ProductRequest>
{
    public ProductValidator()
    {
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}


public class OrderItemValidator : AbstractValidator<OrderItemRequest>
{
    public OrderItemValidator()
    {
        RuleFor(x => x.Product)
            .SetValidator(new ProductValidator());

        RuleFor(x => x.TotalPrice)
            .GreaterThan(0)
            .WithMessage("Total price must be greater than 0.");
    }
}
