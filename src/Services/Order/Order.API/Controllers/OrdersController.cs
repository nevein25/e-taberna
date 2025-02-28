using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Constants;
using Order.Application.Extentions;
using Order.Application.Orders.Commands;
using Order.Application.Orders.DTOs;
using Order.Application.Payments.CreatePayments.Commands;
using Order.Application.Payments.ValidatePayments.Commands;
using Order.Application.Payments.ValidatePayments.DTOs;
using Order.SharedKernel.Messaging;

namespace Order.API.Controllers;
[Route("[controller]")]
[ApiController]

public class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> CreateOrder(OrderRequest orderRequest, CancellationToken cancellationToken)
    {
        orderRequest.CustomerId = User.GetLoggedInUserId();
        var command = new CreateOrderCommand(orderRequest);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Error);

    }



    [HttpPost("create-session")]
    public async Task<IActionResult> CreatePaymentSession(PaymentRequestDto payment)
    {
        var command = new PaymentCommand(payment);
        var result = await _sender.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);


        return Ok(result);
    }

    [HttpPost("validate-payment")]
    public async Task<IActionResult> ValidatePayment(ValidatePaymentRequestDto request)
    {
        var command = new ValidatePaymentCommand(request);
        var result = await _sender.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result);
    }
}
