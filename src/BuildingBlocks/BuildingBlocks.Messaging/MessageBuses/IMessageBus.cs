using RabbitMQ.Client;

namespace BuildingBlocks.Messaging.MessageBuses;
public interface IMessageBus
{
    Task PublishToQueueAsync<T>(T message, string queueName, bool durableQueue = true);
    Task ConsumeFromQueueAsync(string queueName, Func<string, Task> onMessageReceived);
    Task PublishToExchangeAsync<T>(T message, string exchangeName, string routingKey, string type);
    Task ConsumeFromExchangeAsync(string exchangeName, string queueName, string exchangeType, string routingKey, Func<string, Task> onMessageReceived);


}
