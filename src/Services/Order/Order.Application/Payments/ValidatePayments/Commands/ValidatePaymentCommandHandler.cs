using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.Application.Orders.Commands;
using Order.Application.Payments.Interfaces;
using Order.Application.Payments.ValidatePayments.DTOs;
using Order.SharedKernel.CQRS;
using Order.SharedKernel.Results;

namespace Order.Application.Payments.ValidatePayments.Commands;


public class ValidatePaymentCommandHandler : ICommandHandler<ValidatePaymentCommand, ValidatePaymentResponseDto>
{
    private readonly IAppDbContext _context;
    private readonly IPaymentService _paymentService;
    private readonly IPaymentStatusMapper _paymentStatusMapper;

    public ValidatePaymentCommandHandler(IAppDbContext context, IPaymentService paymentService, IPaymentStatusMapper paymentStatusMapper)
    {
        _context = context;
        _paymentService = paymentService;
        _paymentStatusMapper = paymentStatusMapper;
    }

    public async Task<Result<ValidatePaymentResponseDto>> Handle(ValidatePaymentCommand command, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.StripeSessionId == command.Payment.StripeSessionId);

        if (order is null || order.StripeSessionId is null)
            return Result.Failure<ValidatePaymentResponseDto>(Error.NullValue);

        var paymentIntentDto = _paymentService.GetPaymentIntent(order.StripeSessionId);
        order.StripePaymentIntentId = paymentIntentDto.Id;

        var paymentIntentStatus = paymentIntentDto.Status;

        var mappedStatus = _paymentStatusMapper.ConvertFromStripeStatus(paymentIntentStatus);

        order.PaymentStatus = mappedStatus;
        order.StripePaymentIntentId = paymentIntentDto.Id;
        await _context.SaveChangesAsync();

        return new ValidatePaymentResponseDto(paymentIntentStatus);
    }

}
