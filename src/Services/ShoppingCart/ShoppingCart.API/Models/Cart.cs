using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.API.Models;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public string? CouponCode { get; set; }
    public double DiscountPercentage { get; set; }

    public List<CartItem> CartItems { get; set; } = new();

    [NotMapped]
    public decimal TotalPrice => CartItems.Sum(x => x.Price * x.Quantity) * (1 - ((decimal)DiscountPercentage / 100));
}
