namespace Order.Domain.Enums;
public enum PaymentStatus
{
    Canceled,
    Processing,
    RequiresAction,
    RequiresCapture,
    RequiresConfirmation,
    RequiresPaymentMethod,
    Succeeded
}