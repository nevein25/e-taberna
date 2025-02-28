namespace Order.Application.Orders.DTOs;

public class OrderItemRequest
{
    public ProductRequest Product { get; set; } = new();
    public int Quantity { get; set; }

    public decimal TotalPrice
    {
        get => Product.Price * Quantity;
    }
}

