using BuildingBlocks.Messaging.Configurations;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.MessageBuses;
using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.Application.Orders.Commands;
using Order.Application.Payments.Interfaces;
using Order.Application.Payments.ValidatePayments.DTOs;
using Order.Domain.Enums;
using Order.SharedKernel.CQRS;
using Order.SharedKernel.Results;

namespace Order.Application.Payments.ValidatePayments.Commands;


public class ValidatePaymentCommandHandler : ICommandHandler<ValidatePaymentCommand, ValidatePaymentResponseDto>
{
    private readonly IAppDbContext _context;
    private readonly IPaymentService _paymentService;
    private readonly IPaymentStatusMapper _paymentStatusMapper;
    private readonly IMessageBus _messageBus;

    public ValidatePaymentCommandHandler(IAppDbContext context, IPaymentService paymentService,
                                         IPaymentStatusMapper paymentStatusMapper, IMessageBus messageBus)
    {
        _context = context;
        _paymentService = paymentService;
        _paymentStatusMapper = paymentStatusMapper;
        _messageBus = messageBus;
    }

    public async Task<Result<ValidatePaymentResponseDto>> Handle(ValidatePaymentCommand command, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
                            .Include(o => o.OrderItems)
                            .ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.StripeSessionId == command.Payment.StripeSessionId);

        if (order is null || order.StripeSessionId is null)
            return Result.Failure<ValidatePaymentResponseDto>(Error.NullValue);

        var paymentIntentDto = _paymentService.GetPaymentIntent(order.StripeSessionId);
        order.StripePaymentIntentId = paymentIntentDto.Id;

        var paymentIntentStatus = paymentIntentDto.Status;

        var mappedStatus = _paymentStatusMapper.ConvertFromStripeStatus(paymentIntentStatus);

        order.PaymentStatus = mappedStatus;


        if (mappedStatus == PaymentStatus.Succeeded)
            await PublishOrderPaidEventAsync(order);


        order.StripePaymentIntentId = paymentIntentDto.Id;
        await _context.SaveChangesAsync();

        return new ValidatePaymentResponseDto(paymentIntentStatus);
    }

    public async Task PublishOrderPaidEventAsync(Domain.Models.Order order)
    {
        var orderPaidEvent = new OrderPaidEvent
        {
            OrderId = order.Id,
            Products = order.OrderItems.Select(o => new PaidProduct
            {
                Id = o.Product.Id,
                Quantity = o.Product.Quantity
            }).ToList(),
            CustomerId = order.CustomerId
        };

       // await _messageBus.PublishToQueueAsync(orderPaidEvent, QueueNames.OrderPaymentSuccess);
        await _messageBus.PublishToExchangeAsync(orderPaidEvent, ExchangeNames.OrderEvents, "", "fanout");

    }
}
