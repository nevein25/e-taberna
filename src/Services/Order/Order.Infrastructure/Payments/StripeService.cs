using Order.Application.Orders.DTOs;
using Order.Application.Payments.CreatePayments.DTOs;
using Order.Application.Payments.Interfaces;
using Order.Application.Payments.ValidatePayments.DTOs;
using Stripe;
using Stripe.Checkout;

namespace Order.Infrastructure.Payments;
public class StripeService : IPaymentService
{

    private readonly SessionService _sessionService;
    private readonly PaymentIntentService _paymentIntentService;

    public StripeService(SessionService sessionService, PaymentIntentService paymentIntentService)
    {
        _sessionService = sessionService;
        _paymentIntentService = paymentIntentService;
    }
    public StripeResponseDto CreateSession(StripeRequestDto stripeRequest)
    {
        var options = BuildSessionOptions(stripeRequest);

        AddLineItemsToSession(options, stripeRequest.Order.OrderItems);

        Session session = _sessionService.Create(options);

        return new StripeResponseDto()
        {
            SessionId = session.Id,
            SessionUrl = session.Url,
        };
    }
    public PaymentIntentDto GetPaymentIntent(string stripeSessionId)
    {
        Session session = _sessionService.Get(stripeSessionId);
        var paymentIntent = _paymentIntentService.Get(session.PaymentIntentId);
        return new PaymentIntentDto() { Id = session.Id, Status = paymentIntent.Status };
    }

    private SessionCreateOptions BuildSessionOptions(StripeRequestDto stripeRequest)
    {
        return new SessionCreateOptions
        {
            SuccessUrl = stripeRequest.ApprovedUrl,
            CancelUrl = stripeRequest.CancelUrl,
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
        };
    }
    private void AddLineItemsToSession(SessionCreateOptions options, List<OrderItemRequest> orderItems)
    {
        foreach (var item in orderItems)
        {
            var sessionLineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.Product.Price * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Name
                    }
                },
                Quantity = item.Product.Quantity
            };

            options.LineItems.Add(sessionLineItem);
        }
    }
}
