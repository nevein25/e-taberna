using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.API.Models;

public class Cart
{
    public int Id { get; set; }
    public string UserId{ get; set; } = default!;
 
    public List<CartItem> CartItems { get; set; } = new();

    [NotMapped]
    public decimal TotalPrice => CartItems.Sum(x => x.Price * x.Quantity);
}
