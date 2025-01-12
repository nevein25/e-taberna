
using Order.Domain.Enums;

namespace Order.Application.Payments.DTOs;
public class StripeRequestDto
{
    public string ApprovedUrl { get; set; } = default!;
    public string CancelUrl { get; set; } = default!;
    public OrderRequest Order { get; set; } = new();
}

public class StripeResponseDto
{
    public string SessionUrl { get; set; } = default!;
    public string StripeSessionId { get; set; } = default!;
}

public class OrderRequest
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Address { get; set; } = default!;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime OrderTime { get; set; }


    public List<OrderItemRequest> OrderItems { get; set; } = [];
    public decimal TotalPrice
    {
        get => OrderItems.Sum(oi => oi.Product.Price * oi.Product.Quantity);
    }


    public string PaymentIntentId { get; set; } = default!;
    public string StripeSessionId { get; set; } = default!;
}

public class OrderItemRequest
{
    public int Id { get; set; }
    public ProductRequest Product { get; set; } = new();

    public decimal TotalPrice
    {
        get => Product.Price * Product.Quantity;
    }
}

public class ProductRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

