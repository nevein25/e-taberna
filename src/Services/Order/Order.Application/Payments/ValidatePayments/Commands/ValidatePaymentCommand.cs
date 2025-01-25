using FluentValidation;
using Order.Application.Payments.ValidatePayments.DTOs;
using Order.SharedKernel.CQRS;

namespace Order.Application.Payments.ValidatePayments.Commands;

public record ValidatePaymentCommand(ValidatePaymentRequestDto Payment) : ICommand<ValidatePaymentResponseDto>;

public class ValidatePaymetValidator : AbstractValidator<ValidatePaymentCommand>
{
    public ValidatePaymetValidator()
    {
        RuleFor(x => x.Payment.StripeSessionId).NotEmpty().WithMessage("StripeSessionId is required");
    }
}