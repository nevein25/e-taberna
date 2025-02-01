using RabbitMQ.Client;

namespace BuildingBlocks.Messaging.MessageBuses;
public interface IMessageBus
{
    Task PublishToQueueAsync<T>(T message, string queueName, bool durableQueue = true);
    Task ConsumeFromQueueAsync(string queueName, Func<string, Task> onMessageReceived);
}
