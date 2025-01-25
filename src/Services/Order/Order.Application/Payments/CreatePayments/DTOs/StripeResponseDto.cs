namespace Order.Application.Payments.CreatePayments.DTOs;

public class StripeResponseDto
{
    public string SessionUrl { get; set; } = default!;
    public string SessionId { get; set; } = default!;
    //public string PaymentIntentId { get; set; } = default!;
}

