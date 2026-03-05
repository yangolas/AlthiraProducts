using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.MessageBrokerConsumer;

namespace AlthiraProducts.Products.Application.Ports.MessageBrokerConsumer;

public interface IProductConsumerService : IConsumerService<Event>
{
}
