namespace Order.Domain.Models;
public class OrderItem
{
    public int Id { get; set; }
    public Product Product { get; set; } = new();
    public int Quantity { get; set; }

    public decimal TotalPrice
    {
        get => Product.Price * Quantity;
    }
}
