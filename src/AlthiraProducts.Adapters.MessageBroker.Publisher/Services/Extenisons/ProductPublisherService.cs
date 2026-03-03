using AlthiraProducts.Adapters.MessageBroker.Events.Models;
using AlthiraProducts.Adapters.MessageBroker.Publisher.Ports.Extensions;
using AlthiraProducts.Main.Settings.Models;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.Services;

public class ProductPublisherService : PublisherService<Event>,IProductPublisherService
{
    public ProductPublisherService(
        MessageBrokerSettings messageBrokerSettings,
        ChannelSettings[] channelSettings)
        : base(
            messageBrokerSettings,
            channelSettings)
    {
    }
}