namespace Order.Application.Payments.ValidatePayments.DTOs;
public class PaymentIntentDto
{
    public string Id { get; set; } = default!;
    public string Status { get; set; } = default!;
}
