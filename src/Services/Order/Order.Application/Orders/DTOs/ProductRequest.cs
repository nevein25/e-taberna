namespace Order.Application.Orders.DTOs;

public class ProductRequest
{
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

