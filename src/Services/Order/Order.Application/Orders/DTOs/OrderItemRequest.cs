namespace Order.Application.Orders.DTOs;

public class OrderItemRequest
{
    public ProductRequest Product { get; set; } = new();

    public decimal TotalPrice
    {
        get => Product.Price * Product.Quantity;
    }
}

