using Order.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Domain.Models;
public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Address { get; set; } = default!;
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

    [Column(TypeName = "nvarchar(50)")]
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.RequiresPaymentMethod;
    public DateTime OrderTime { get; set; }

    public string? DiscountCode { get; set; }
    public double DiscountPercentage { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];

    public decimal TotalPrice
    {
        get => OrderItems.Sum(oi => oi.Product.Price * oi.Quantity) * (1 - ((decimal)DiscountPercentage / 100));
    }

    public string? StripePaymentIntentId { get; set; } // The payment intent ID gets generated once the checkout is done.
                                                       //so we can provide a refund or tracking if the payment was successful.

    public string? StripeSessionId { get; set; }
}
