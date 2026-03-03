using AlthiraProducts.Adapters.MessageBroker.Events.Models;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.Ports.Extensions;

public interface IProductConsumerService : IConsumerService<Event>
{
}
