using AlthiraProducts.Adapters.MessageBroker.Consumer.Ports.Extensions;
using AlthiraProducts.Adapters.MessageBroker.Events.Models;
using AlthiraProducts.Main.Settings.Models;
using MediatR;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.Services.Extensions;

public class ProductConsumerService : ConsumerService<Event>, IProductConsumerService
{
    public ProductConsumerService(
        MessageBrokerSettings messageBrokerSettings,
        ChannelSettings channelSettings,
        IMediator mediator)
        : base(messageBrokerSettings, channelSettings, mediator)
    {
    }
}
