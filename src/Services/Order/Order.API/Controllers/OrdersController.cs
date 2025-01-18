using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Constants;
using Order.Application.Extentions;
using Order.Application.Orders.Commands;
using Order.Application.Orders.DTOs;
using Order.SharedKernel.Messaging;
using Order.SharedKernel.Results;

namespace Order.API.Controllers;
[Route("api/[controller]")]
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
}
