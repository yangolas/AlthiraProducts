using AlthiraProducts.BuildingBlocks.Application.EventModel;

namespace AlthiraProducts.BuildingBlocks.Application.Ports.MessageBrokerPublisher;

public interface IPublisherService<TEvent> where TEvent : Event
{
    Task PublishAsync(TEvent @event);

    IEnumerable<string> GetEventsName();
}
