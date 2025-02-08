using Order.Domain.Enums;

namespace Order.Application.Orders.DTOs;

public class OrderRequest
{
    public int CustomerId { get; set; }
    public string Address { get; set; } = default!;
    public string? DiscountCode { get; set; }
    public double DiscountPercentage { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime OrderTime { get; set; }
    public List<OrderItemRequest> OrderItems { get; set; } = [];
    public decimal TotalPrice { get; set; }

}

