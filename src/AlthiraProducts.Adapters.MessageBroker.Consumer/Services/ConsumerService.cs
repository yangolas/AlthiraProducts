using AlthiraProducts.Adapters.MessageBroker.Consumer.Ports;
using AlthiraProducts.Adapters.MessageBroker.Events.Models;
using AlthiraProducts.Main.Settings.Models;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.Services;

public class ConsumerService<TEvent> : RabbitConsumerContext, IConsumerService<TEvent> where TEvent : Event
{
    private readonly IMediator _mediator;
    public ConsumerService(
        MessageBrokerSettings messageBrokerSettings,
        ChannelSettings channelSettings,
        IMediator mediator)
        : base(
            messageBrokerSettings,
            channelSettings)
    {
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

        return @event;
    }

    public async override Task OnReceivedAsync(object sender, BasicDeliverEventArgs @event)
    {
        IChannel channel = ((AsyncEventingBasicConsumer)sender).Channel;

        try
        {
            string message = Encoding.UTF8.GetString(@event.Body.Span);

            TEvent eventBase = JsonSerializer.Deserialize<TEvent>(message)
                ?? throw new Exception("Error in the deserializer of event base");

            Type eventType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == eventBase.EventName)
                ?? throw new Exception($"Cannot resolve event type: {eventBase.EventName}");

            object specificEvent = JsonSerializer.Deserialize(message, eventType)
                ?? throw new Exception("Specific event deserialization failed");

            await _mediator.Send((dynamic)specificEvent);

            await channel.BasicAckAsync(@event.DeliveryTag, multiple: false);
        }
        catch (Exception)
        {
            await RequeueInRetryOrDeadLetterAsync(channel, @event);
        }
    }
}
