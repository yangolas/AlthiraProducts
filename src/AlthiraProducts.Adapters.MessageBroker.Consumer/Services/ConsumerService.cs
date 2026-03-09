using AlthiraProducts.Adapters.MessageBroker.Consumer.Diagnostic.Telemetry;
using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.MessageBrokerConsumer;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.Services;

public class ConsumerService<TEvent> : RabbitConsumerContext, IConsumerService<TEvent> where TEvent : Event
{
    private readonly ILogger<ConsumerService<TEvent>> _logger;
    private readonly IOpenTelemetryService _openTelemetryService;
    private readonly IMediator _mediator;
    public ConsumerService(
        ILogger<ConsumerService<TEvent>> logger,
        IOpenTelemetryService openTelemetryService,
        MessageBrokerSettings messageBrokerSettings,
        ChannelSettings channelSettings,
        IMediator mediator)
        : base(
            logger,
            openTelemetryService,
            messageBrokerSettings,
            channelSettings)
    {
        _logger = logger;
        _openTelemetryService = openTelemetryService;
        _mediator = mediator;
    }

    public override async Task ConsumeMediatRAsync(CancellationToken stoppingToken)
    {
        await base.ConsumeMediatRAsync(stoppingToken);
    }

    public async Task<TEventOut> ConsumeAsync<TEventOut>() where TEventOut : Event
    {
        BasicDeliverEventArgs eventArgs = await base.ConsumeAsync(CancellationToken.None);
        string message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

        TEventOut @event = JsonSerializer.Deserialize<TEventOut>(message)
           ?? throw new Exception("Error in the deserializer of specific event, check if template is type Event");

        _openTelemetryService.AddConsumerBrokerMetadata(@event);
        _openTelemetryService.AddStep($"Event base deserialized: {@event.EventName}");

        return @event;
    }

    public async override Task OnReceivedAsync(object sender, BasicDeliverEventArgs @event)
    {
        IChannel channel = ((AsyncEventingBasicConsumer)sender).Channel;

        _openTelemetryService.AddStep($"Message received from RabbitMQ: {@event.RoutingKey}");
       
        IDictionary<string,object?>? headers = @event.BasicProperties.Headers;
        if (headers != null)
            _openTelemetryService.ExtractContext(headers);

        try
        {
            string message = Encoding.UTF8.GetString(@event.Body.Span);
            _logger.LogInformation("Processing message from routing key: {RoutingKey}", @event.RoutingKey);

            TEvent eventBase = JsonSerializer.Deserialize<TEvent>(message)
                ?? throw new Exception("Error in the deserializer of event base");
            _openTelemetryService.AddStep($"Event base deserialized: {eventBase.EventName}");


            Type eventType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == eventBase.EventName)
                ?? throw new Exception($"Cannot resolve event type: {eventBase.EventName}");

            object specificEvent = JsonSerializer.Deserialize(message, eventType)
                ?? throw new Exception("Specific event deserialization failed");

            _openTelemetryService.AddStep($"Sending event to: {eventBase.EventName}Handler");
            await _mediator.Send((dynamic)specificEvent);

            await channel.BasicAckAsync(@event.DeliveryTag, multiple: false);

            _openTelemetryService.AddConsumerBrokerMetadata(eventBase, @event);
            _logger.LogInformation("Message {EventName} processed and acknowledged", eventBase.EventName);
            _openTelemetryService.AddStep("Message acknowledged successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical error processing Event {EventId}", @event.BasicProperties.MessageId ?? "Unknown");
            _openTelemetryService.AddError(ex, $"Error processing Event {@event.BasicProperties.MessageId ?? "Unknown"}");
            await RequeueInRetryOrDeadLetterAsync(channel, @event);
        }
    }
}
