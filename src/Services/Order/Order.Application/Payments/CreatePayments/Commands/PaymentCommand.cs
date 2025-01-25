using FluentValidation;
using Order.SharedKernel.CQRS;

namespace Order.Application.Payments.CreatePayments.Commands;
public record PaymentCommand(PaymentRequestDto Payment) : ICommand<PaymentResponseDto>;


public record PaymentRequestDto(string ApprovedUrl, string CancelUrl, int OrderId);

public record PaymentResponseDto(string SessionUrl, string StripeSessionId);


public class PaymentCommandValidator : AbstractValidator<PaymentCommand>
{
    public PaymentCommandValidator()
    {
        RuleFor(x => x.Payment.ApprovedUrl).NotEmpty().WithMessage("ApprovedUrl is required");
        RuleFor(x => x.Payment.CancelUrl).NotEmpty().WithMessage("CancelUrl is required");
        RuleFor(x => x.Payment.OrderId).NotEmpty().WithMessage("OrderId is required");


    }
}
