using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.MessageBrokerPublisher;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.Services;

public class PublisherService<TEvent> : RabbitPublishContext, IPublisherService<TEvent> where TEvent : Event
{
    public PublisherService(
       ILogger<PublisherService<TEvent>> logger,
       IOpenTelemetryService openTelemetryService,
       MessageBrokerSettings messageBrokerSettings,
       ChannelSettings[] channelSettings)
        : base(
            logger,
            openTelemetryService,
            messageBrokerSettings,
            channelSettings)
    {
    }

    public async Task PublishAsync(TEvent @event)
    {
        await base.PublishAsync(@event);
    }
}
