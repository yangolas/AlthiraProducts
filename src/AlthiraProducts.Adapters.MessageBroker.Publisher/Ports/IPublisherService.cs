using AlthiraProducts.Adapters.MessageBroker.Events.Models;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.Ports;

public interface IPublisherService<TEvent> where TEvent : Event
{
    Task PublishAsync(TEvent @event);

    IEnumerable<string> GetEventsName();
}
