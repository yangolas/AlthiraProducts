using AlthiraProducts.Adapters.MessageBroker.Events.Models;
using AlthiraProducts.Adapters.MessageBroker.Publisher.Ports;
using AlthiraProducts.Main.Settings.Models;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.Services;

public class PublisherService<TEvent> : RabbitPublishContext, IPublisherService<TEvent> where TEvent : Event
{
    public PublisherService(
       MessageBrokerSettings messageBrokerSettings,
       ChannelSettings[] channelSettings)
        : base(
            messageBrokerSettings,
            channelSettings)
    {
    }

    public async Task PublishAsync(TEvent @event)
    {
        await base.PublishAsync(@event);
    }
}
