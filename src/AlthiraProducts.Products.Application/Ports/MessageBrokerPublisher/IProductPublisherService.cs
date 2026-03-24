using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.MessageBrokerPublisher;

namespace AlthiraProducts.Products.Application.Ports.MessageBrokerPublisher;

public interface IProductPublisherService : IPublisherService<Event>
{
}