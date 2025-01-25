using Order.Application.Payments.Interfaces;
using Order.Domain.Enums;
using System;

namespace Order.Infrastructure.Payments;
public class PaymentStatusMapper : IPaymentStatusMapper
{
    public PaymentStatus ConvertFromStripeStatus(string stripeStatus)
    {
        return stripeStatus.ToLower() switch
        {
            "canceled" => PaymentStatus.Canceled,
            "processing" => PaymentStatus.Processing,
            "requires_action" => PaymentStatus.RequiresAction,
            "requires_capture" => PaymentStatus.RequiresCapture,
            "requires_confirmation" => PaymentStatus.RequiresConfirmation,
            "requires_payment_method" => PaymentStatus.RequiresPaymentMethod,
            "succeeded" => PaymentStatus.Succeeded,
            _ => throw new ArgumentException($"Invalid payment status: {stripeStatus}")
        };
    }

    public string ConvertToStripeStatus(PaymentStatus status)
    {
        return status switch
        {
            PaymentStatus.Canceled => "canceled",
            PaymentStatus.Processing => "processing",
            PaymentStatus.RequiresAction => "requires_action",
            PaymentStatus.RequiresCapture => "requires_capture",
            PaymentStatus.RequiresConfirmation => "requires_confirmation",
            PaymentStatus.RequiresPaymentMethod => "requires_payment_method",
            PaymentStatus.Succeeded => "succeeded",
            _ => throw new ArgumentException($"Invalid payment status: {status}")
        };
    }
}

