using Order.Domain.Enums;

namespace Order.Application.Orders.DTOs;

public class OrderRequest
{
    public int CustomerId { get; set; }
    public string Address { get; set; } = default!;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime OrderTime { get; set; }


    public List<OrderItemRequest> OrderItems { get; set; } = [];
    public decimal TotalPrice
    {
        get => OrderItems.Sum(oi => oi.Product.Price * oi.Product.Quantity);
    }

}

