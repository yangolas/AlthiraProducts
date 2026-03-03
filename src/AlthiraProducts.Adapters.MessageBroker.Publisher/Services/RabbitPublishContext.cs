using AlthiraProducts.Adapters.MessageBroker.Events.Models;
using AlthiraProducts.Main.Settings.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.Services;

public abstract class RabbitPublishContext
{
    #pragma warning disable CS8618
    private static IConnection _connection;
    #pragma warning restore CS8618
    private readonly ChannelSettings[] _channelsSettings;
    private readonly string _exchangeName;
    private readonly Dictionary<string, string> _routingKeysByEventName;
    public RabbitPublishContext(
        MessageBrokerSettings messageBrokerSettings,
        ChannelSettings[] channelSettings)
    {
        _channelsSettings = channelSettings;
        _exchangeName = _channelsSettings.First().Exchange;

        _routingKeysByEventName = _channelsSettings
        .ToDictionary(channel => 
            channel.EventName, 
            channel => channel.RoutingKey);

        if (_connection == null)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = messageBrokerSettings.HostName,
                UserName = messageBrokerSettings.UserName,
                Password = messageBrokerSettings.Password,
                Port = messageBrokerSettings.Port
            };

            _connection = Task.Run(async () =>
                await connectionFactory.CreateConnectionAsync()
            ).Result;
        }
        if (_connection is null)
            throw new Exception("Broker can't publish, check conectivity or rabbit mq is activated");
    }

    private async Task<IChannel> CreateChannelAsync()
    {
        IChannel channel = await _connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            _exchangeName,
            ExchangeType.Direct,
            durable: true
        );

        foreach (var channelSetting in _channelsSettings)
        {
            await channel.QueueDeclareAsync(
               queue: channelSetting.Queue,
               durable: true,
               exclusive: false,
               autoDelete: false
            );

            await channel.QueueBindAsync(
                queue: channelSetting.Queue,
                exchange: _exchangeName,
                routingKey: channelSetting.RoutingKey
            );
        }

        return channel;
    }
    public IEnumerable<string> GetEventsName() => _channelsSettings.Select(c => c.EventName);
    public virtual async Task PublishAsync<TEvent>(TEvent @event) where TEvent : Event
    {
        try
        {
            IChannel channel = await CreateChannelAsync();

            string message = JsonSerializer.Serialize(@event);
            byte[] body = Encoding.UTF8.GetBytes(message);

            BasicProperties properties = new()
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent
            };

            if (!_routingKeysByEventName.TryGetValue(@event.EventName, out string? routingKey) 
                || string.IsNullOrEmpty(routingKey))
            {
                throw new Exception($"No routing key configured for event type '{@event.EventName}'");
            }

            await channel.BasicPublishAsync(
                exchange: _exchangeName,
                routingKey: routingKey!,
                mandatory: false,
                properties,
                new ReadOnlyMemory<byte>(body)
            );

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}