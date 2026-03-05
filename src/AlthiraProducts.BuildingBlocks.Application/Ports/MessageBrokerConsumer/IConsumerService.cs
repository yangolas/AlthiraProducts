using AlthiraProducts.BuildingBlocks.Application.EventModel;

namespace AlthiraProducts.BuildingBlocks.Application.Ports.MessageBrokerConsumer;

public interface IConsumerService<TEvent> where TEvent : Event
{
    Task ConsumeMediatRAsync(CancellationToken stoppingToken);
    Task<TEventOut> ConsumeAsync<TEventOut>() where TEventOut : Event;
    string EventName { get; }
}