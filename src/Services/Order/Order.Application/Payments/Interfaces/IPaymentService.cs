using Order.Application.Payments.DTOs;

namespace Order.Application.Payments.Interfaces;
public interface IPaymentService
{
    StripeResponseDto CreateSession(StripeRequestDto stripeRequest);
    bool IsPaymentSuccessful(string stripeSessionId);
}
