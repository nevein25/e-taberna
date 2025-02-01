using BuildingBlocks.Messaging.Configurations;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.MessageBuses;
using System.Text.Json;

namespace ProductCatalog.API.Products.AdjustInventoryOnOrderPaid;

public class OrderPaidEventConsumer : BackgroundService //allows the application to start a background worker that keeps running independently from API requests, listening for events from the queue.
{
    private readonly IMessageBus _messageBus;
    private readonly IServiceProvider _serviceProvider;

    public OrderPaidEventConsumer(IMessageBus messageBus, IServiceProvider serviceProvider)
    {
        _messageBus = messageBus;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        _messageBus.ConsumeFromQueueAsync(QueueNames.OrderPaymentSuccess, async message =>
        {
            await HandleMessageAsync(message);
        });

        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(string message)
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AdjustInventoryHandler>();


        var orderPaidEvent = JsonSerializer.Deserialize<OrderPaidEvent>(message);
        if (orderPaidEvent is not null)
            await handler.HandleAsync(orderPaidEvent);

    }
}