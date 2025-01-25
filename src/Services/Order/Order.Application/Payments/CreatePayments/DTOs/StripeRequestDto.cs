using Order.Application.Orders.DTOs;

namespace Order.Application.Payments.CreatePayments.DTOs;
public class StripeRequestDto
{
    public string ApprovedUrl { get; set; } = default!;
    public string CancelUrl { get; set; } = default!;

    public OrderRequest Order { get; set; } = new();
}

