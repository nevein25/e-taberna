namespace ShoppingCart.API.Models;

public class CartItem
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}