using BuildingBlocks.Messaging.Configurations;
using Microsoft.Extensions.Options;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace BuildingBlocks.Messaging.MessageBuses;
public class RabbitMQMessageBus : IMessageBus, IAsyncDisposable
{
    private IConnection? _connection; //  represents an AMQP 0-9-1 connection
    private IChannel? _channel; // represents an AMQP 0-9-1 channel, and provides most of the operations (protocol methods)
    private readonly RabbitMQConfigurations _options;

    public RabbitMQMessageBus(IOptions<RabbitMQConfigurations> options)
    {
        _options = options.Value ?? throw new ArgumentNullException("can not find RabbitMQConfigurations in appsettings");
    }

    public async Task PublishToQueueAsync<T>(T message, string queueName, bool durableQueue = true)
    {

        await EnsureConnectionAsync();

        if (_channel is null) throw new InvalidOperationException("Channel is not initialized");

        await _channel.QueueDeclareAsync( //ensures that the queue exists. If it doesn't, it will be created with the specified parameters
         queue: queueName,
            durable: durableQueue,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            body: body
        );

    }

    public async Task ConsumeFromQueueAsync(string queueName, Func<string, Task> onMessageReceived)
    {
        await EnsureConnectionAsync();

        if (_channel is null) throw new InvalidOperationException("Channel is not initialized");

        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (channel, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);

                await onMessageReceived(message);

                await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            }
            catch
            {
                await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer);
    }
    public async Task PublishToExchangeAsync<T>(T message, string exchangeName, string routingKey, string type)
    {
        await EnsureConnectionAsync();

        if (_channel is null) throw new InvalidOperationException("Channel is not initialized");

        byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await _channel.ExchangeDeclareAsync(exchange: exchangeName, type: type, durable: true);

        await _channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            body: body
        );
    }

    public async Task ConsumeFromExchangeAsync(string exchangeName, string queueName, string exchangeType, string routingKey, Func<string, Task> onMessageReceived)
    {
        await EnsureConnectionAsync();

        if (_channel is null) throw new InvalidOperationException("Channel is not initialized");

        await _channel.ExchangeDeclareAsync(exchange: exchangeName, type: exchangeType, durable: true);

        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        if (exchangeType == "fanout")
            await _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: "");

        else
            await _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (channel, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);

                await onMessageReceived(message);

                await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            }
            catch
            {
                await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer);
    }

    private async Task CreateConnectionAsync()
    {
        ConnectionFactory factory = new ConnectionFactory // constructs IConnection instances
        {
            HostName = _options.HostName,
            Password = _options.Password,
            UserName = _options.UserName
        };

        _connection = await factory.CreateConnectionAsync();
    }


    private async Task EnsureConnectionAsync()
    {
        if (_connection is null || !_connection.IsOpen)
            await CreateConnectionAsync();

        if ((_channel is null || !_channel.IsOpen) && _connection is not null)
            _channel = await _connection.CreateChannelAsync(); // The IConnection interface can then be used to open a channel:

    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
            _channel.Dispose();
        }

        if (_connection is not null)
        {
            await _connection.CloseAsync(); // if you closed connection, channle will be closed already by the connection but the best practice is to close it yourself
            _connection.Dispose();
        }
    }
}

/*
   1. create ConnectionFactory obj

       ConnectionFactory factory = new ConnectionFactory // constructs IConnection instances
        {
            HostName = _options.HostName,
            Password = _options.Password,
            UserName = _options.UserName
        }; 

   2. create connection
     private IConnection?  _connection = await factory.CreateConnectionAsync();
 

   3. open channel
      IChannel _channel = await _connection.CreateChannelAsync(); 


   4.  declare quesue

        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: durableQueue,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

   4. push message to the queue
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            body: body
        );

    5. close channel and connection 

    _channel.CloseAsync();
    _connection.CloseAsync();
 */