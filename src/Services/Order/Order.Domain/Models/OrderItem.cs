namespace Order.Domain.Models;
public class OrderItem
{
    public int Id { get; set; }
    public Product Product { get; set; } = new();

    public decimal TotalPrice
    {
        get => Product.Price * Product.Quantity;
    }
}
