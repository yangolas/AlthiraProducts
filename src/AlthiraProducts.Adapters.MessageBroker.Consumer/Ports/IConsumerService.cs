using AlthiraProducts.Adapters.MessageBroker.Events.Models;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.Ports;

public interface IConsumerService<TEvent> where TEvent : Event
{
    Task ConsumeMediatRAsync(CancellationToken stoppingToken);
    Task<TEventOut> ConsumeAsync<TEventOut>() where TEventOut : Event;
    string EventName { get; }
}