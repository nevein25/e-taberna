using BuildingBlocks.Messaging.Configurations;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.MessageBuses;
using ShoppingCart.API.ShoppingCart.UpdateCart;
using System.Text.Json;

namespace ShoppingCart.API.ShoppingCart.DeleteCart;

public class DeleteCartOnOrderPaidConsumer : BackgroundService //allows the application to start a background worker that keeps running independently from API requests, listening for events from the queue.
{
    private readonly IMessageBus _messageBus;
    private readonly IServiceProvider _serviceProvider;

    public DeleteCartOnOrderPaidConsumer(IMessageBus messageBus, IServiceProvider serviceProvider)
    {
        _messageBus = messageBus;
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        //_messageBus.ConsumeFromQueueAsync(QueueNames.OrderPaymentSuccess, async message =>
        //{
        //    await HandleMessageAsync(message);
        //});
        await _messageBus.ConsumeFromExchangeAsync(ExchangeNames.OrderEvents, QueueNames.CartOrderPaymentSuccess,
            "fanout", "", HandleMessageAsync);

        //return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(string message)
    {
        var scope = _serviceProvider.CreateScope();
        var cartDeletionService = scope.ServiceProvider.GetRequiredService<ICartDeletionService>();
        var orderPaidEvent = JsonSerializer.Deserialize<OrderPaidEvent>(message);
        if (orderPaidEvent is not null)
            await cartDeletionService.DeleteCartAsync(orderPaidEvent.CustomerId, CancellationToken.None);

    }
}