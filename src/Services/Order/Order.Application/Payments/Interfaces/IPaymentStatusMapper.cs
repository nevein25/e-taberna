using Order.Domain.Enums;

namespace Order.Application.Payments.Interfaces;
public interface IPaymentStatusMapper
{
    string ConvertToStripeStatus(PaymentStatus status);
    PaymentStatus ConvertFromStripeStatus(string stripeStatus);
}
