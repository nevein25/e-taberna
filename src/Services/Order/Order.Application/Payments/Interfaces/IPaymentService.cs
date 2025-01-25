using Order.Application.Payments.CreatePayments.DTOs;
using Order.Application.Payments.ValidatePayments.DTOs;

namespace Order.Application.Payments.Interfaces;
public interface IPaymentService
{
    StripeResponseDto CreateSession(StripeRequestDto stripeRequest);
    PaymentIntentDto GetPaymentIntent(string stripeSessionId);
}
