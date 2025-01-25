using Mapster;
using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.Application.Orders.DTOs;
using Order.Application.Payments.CreatePayments.DTOs;
using Order.Application.Payments.Interfaces;
using Order.SharedKernel.CQRS;
using Order.SharedKernel.Results;

namespace Order.Application.Payments.CreatePayments.Commands;
public class PaymentHandler : ICommandHandler<PaymentCommand, PaymentResponseDto>
{
    private readonly IPaymentService _paymentService;
    private readonly IAppDbContext _context;

    public PaymentHandler(IPaymentService paymentService, IAppDbContext context)
    {
        _paymentService = paymentService;
        _context = context;
    }

    public async Task<Result<PaymentResponseDto>> Handle(PaymentCommand command, CancellationToken cancellationToken)
    {
        // TODO
        // change inventory (product service)
        // delete cart
        var order = await _context.Orders.Include(o => o.OrderItems)
                                          .ThenInclude(oi => oi.Product)
                                          .FirstOrDefaultAsync(o => o.Id == command.Payment.OrderId);

        if (order is null)
            return Result.Failure<PaymentResponseDto>(Error.NullValue);

        var stripeRequest = new StripeRequestDto
        {
            ApprovedUrl = command.Payment.ApprovedUrl,
            CancelUrl = command.Payment.CancelUrl,
            Order = order.Adapt<OrderRequest>()
        };

        var stripeResponse = _paymentService.CreateSession(stripeRequest);

        order.StripeSessionId = stripeResponse.SessionId;
        
        await _context.SaveChangesAsync();

        return new PaymentResponseDto(stripeResponse.SessionUrl, stripeResponse.SessionId);
    }
}
