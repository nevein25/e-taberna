using Order.Application.Payments.DTOs;
using Order.Application.Payments.Interfaces;
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
        // TODO: store sessionId in the database, so we can provide a refund or tracking if the payment was successful.
        var options = BuildSessionOptions(stripeRequest);

        AddLineItemsToSession(options, stripeRequest.Order.OrderItems);

        Session session = _sessionService.Create(options);

        return new StripeResponseDto()
        {
            StripeSessionId = session.Id,
            SessionUrl = session.Url
        };
    }

    public bool IsPaymentSuccessful(string stripeSessionId)
    {

        Session session = _sessionService.Get(stripeSessionId);
        PaymentIntent paymentIntent = _paymentIntentService.Get(session.PaymentIntentId);
        return paymentIntent.Status == "succeeded";
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
