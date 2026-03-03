using AlthiraProducts.Adapters.MessageBroker.Events.Models;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.Ports.Extensions;

public interface IProductPublisherService : IPublisherService<Event>
{
}