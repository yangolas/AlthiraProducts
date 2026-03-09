using AlthiraProducts.Adapters.MessageBroker.Consumer.Diagnostic.Telemetry;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.Services;

public abstract class RabbitConsumerContext
{
#pragma warning disable CS8618
    private static IConnection _connection;
#pragma warning restore CS8618
    private readonly ILogger _logger;
    private readonly IOpenTelemetryService _openTelemetryService;
    private readonly ChannelSettings _channelSettings;
    private readonly int _maxRetryAttempts;
    private readonly TimeSpan _initialDelay;
    private readonly double _backoffFactor;
    public string EventName { get; init; }
    public RabbitConsumerContext(
        ILogger logger,
        IOpenTelemetryService openTelemetryService,
        MessageBrokerSettings messageBrokerSettings,
        ChannelSettings channelSettings)
    {
        _maxRetryAttempts = channelSettings.RetryPolicy.MaxRetryAttempts;
        _initialDelay = channelSettings.RetryPolicy.InitialDelay;
        _backoffFactor = channelSettings.RetryPolicy.BackoffMultiplier;
        _channelSettings = channelSettings;
        _logger = logger;
        _openTelemetryService = openTelemetryService;
        EventName = channelSettings.EventName;
        if (_connection is null)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = messageBrokerSettings.HostName,
                UserName = messageBrokerSettings.UserName,
                Password = messageBrokerSettings.Password,
                Port = messageBrokerSettings.Port,
                ConsumerDispatchConcurrency = (ushort)messageBrokerSettings.ConsumerConcurrency,
            };

            _connection = Task.Run(async () =>
                await connectionFactory.CreateConnectionAsync()
            ).Result;
        }
        if (_connection is null)
            throw new Exception("Broker can't consume, check conectivity or rabbit mq is activated");
    }

    /// <summary>
    /// [Main Queue] --fail--> [Retry Queue1] --delay--> [Main queue] --fail--> [[Retry Queue2] --delay--> [Main queue] --> [N times ...] --> [DLQ final]
    /// </summary>
    /// <returns></returns>
    private async Task<IChannel> CreateChannelAsync()
    {
        IChannel channel = await _connection.CreateChannelAsync();
        
        //Exchange Main queue
        await channel.ExchangeDeclareAsync(
            _channelSettings.Exchange,
            ExchangeType.Direct,
            durable: true
        );

        //Exchange for retry queue and dead letter queue
        var dlxExchange = $"{_channelSettings.Exchange}.dlx";
        await channel.ExchangeDeclareAsync(
            dlxExchange,
            ExchangeType.Direct,
            durable: true
        );

        //Configuration of Main Queue

        await channel.QueueDeclareAsync(
            queue: _channelSettings.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        //Configuration of Retry Queues

        for (int index = 0; index < _maxRetryAttempts; index++)
        {
            var retryLevel = index + 1;

            string retryQueueName = $"{_channelSettings.Queue}.retry.{retryLevel}";

            var delay = TimeSpan.FromMilliseconds(
                _initialDelay.TotalMilliseconds *
                Math.Pow(_backoffFactor, index)
            );

            await channel.QueueDeclareAsync(
                queue: retryQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object?>
                {
                    { "x-message-ttl", (long)delay.TotalMilliseconds },
                    { "x-dead-letter-exchange", _channelSettings.Exchange },
                    { "x-dead-letter-routing-key", _channelSettings.RoutingKey }
                }
            );
        }


        //Configuration of Dead Letter Queue
        await channel.QueueDeclareAsync(
            queue: $"{_channelSettings.Queue}.dlq",
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        //Binding Main Queue to Main Exchange
        await channel.QueueBindAsync(
           queue: _channelSettings.Queue,
           exchange: _channelSettings.Exchange,
           routingKey: _channelSettings.RoutingKey
       );

        //Binding Retry Queue to DLX Exchange
        for (int index = 0; index < _maxRetryAttempts; index++)
        {
            var retryLevel = index + 1;
            await channel.QueueBindAsync(
                queue: $"{_channelSettings.Queue}.retry.{retryLevel}",
                exchange: dlxExchange,
                routingKey: $"{_channelSettings.RoutingKey}.retry.{retryLevel}"
            );
        }

        //Binding Dead letter Queue to Dead Letter Exchange
        await channel.QueueBindAsync(
            queue: $"{_channelSettings.Queue}.dlq",
            exchange: dlxExchange,
            routingKey: $"{_channelSettings.RoutingKey}.dlq"
        );

        return channel;
    }

    private static int GetRetryCount(BasicDeliverEventArgs args)
    {
        if (args.BasicProperties?.Headers == null)
            return 0;

        if (!args.BasicProperties.Headers.TryGetValue("x-death", out var deathHeader))
            return 0;

        var deaths = deathHeader as List<object>;
        if (deaths == null || deaths.Count == 0)
            return 0;

        if (deaths[0] is not Dictionary<string, object> death || !death.TryGetValue("count", out var count))
            return 0;

        return Convert.ToInt32(count);
    }

    public virtual async Task ConsumeMediatRAsync(CancellationToken stoppingToken)
    {
        try
        {
            IChannel channel = await CreateChannelAsync();

            AsyncEventingBasicConsumer consumer = new(channel);

            consumer.ReceivedAsync += OnReceivedAsync;

            await channel.BasicConsumeAsync(
                queue: _channelSettings.Queue,
                autoAck: false,
                consumer: consumer
            );

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }

    public virtual async Task<BasicDeliverEventArgs> ConsumeAsync(CancellationToken stoppingToken)
    {
        try
        {
            IChannel channel = await CreateChannelAsync();
            AsyncEventingBasicConsumer consumer = new(channel);

            TaskCompletionSource<BasicDeliverEventArgs> task = new();

            consumer.ReceivedAsync += (sender, args) =>
            {
                task.TrySetResult(args);
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(
                queue: _channelSettings.Queue,
                autoAck: true,
                consumer: consumer
            );

            using (stoppingToken.Register(() => task.TrySetCanceled()))
            {
                return await task.Task;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }

    public async Task RequeueInRetryOrDeadLetterAsync(IChannel channel, BasicDeliverEventArgs @event) 
    {
        int retryCount = GetRetryCount(@event);
        string dlxExchange = $"{_channelSettings.Exchange}.dlx";
        int nextRetryLevel = retryCount + 1;

        _openTelemetryService.AddStep($"Processing message failure. Current retries: {retryCount}");

        if (nextRetryLevel <= _maxRetryAttempts)
        {
            string nextRetryRoutingKey = $"{_channelSettings.RoutingKey}.retry.{nextRetryLevel}";
            _openTelemetryService.AddConsumerBrokerRetryMetadata(@event, nextRetryLevel, nextRetryRoutingKey);

            // Publish meesage to next retry queue
            await channel.BasicPublishAsync(
                exchange: dlxExchange,
                routingKey: nextRetryRoutingKey,
                mandatory: false,
                basicProperties: new BasicProperties 
                {
                    Headers = @event.BasicProperties.Headers,
                    CorrelationId = @event.BasicProperties.CorrelationId,
                    DeliveryMode = @event.BasicProperties.DeliveryMode,
                },
                body: @event.Body
            );

            // ACK message in mainqueue to remove it
            await channel.BasicAckAsync(@event.DeliveryTag, multiple: false);

            _logger.LogError("Message failed. Sent to retry {nextRetryLevel}", nextRetryLevel);
            _openTelemetryService.AddStep($"Message moved to retry queue: {nextRetryLevel}");
        }
        else
        {
            _openTelemetryService.AddError("Max retry attempts reached");
            // Max retries → Deade letter queue
            await channel.BasicNackAsync(@event.DeliveryTag, multiple: false, requeue: false);
            
            _openTelemetryService.AddConsumerBrokerDLQMetadata(@event, _maxRetryAttempts);
            _logger.LogCritical("Message failed permanently. Max retries reached ({MaxRetries}). Sent to DLQ. CorrelationId: {CorrelationId}",
            _maxRetryAttempts, @event.BasicProperties.CorrelationId);

            _openTelemetryService.AddStep("Message sent to DLQ (Permanent Failure)");

        }
    }
    public abstract Task OnReceivedAsync(object sender, BasicDeliverEventArgs @event);
}