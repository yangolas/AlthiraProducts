using AlthiraProducts.Adapters.MessageBroker.Events.Models;
using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;

namespace AlthiraProducts.Adapters.Outbox.Mappers;

public static class Mapper_OutBoxEvent_Event
{
    public static Event MapToEvent(this OutboxEventWriteModel outboxEvent)
    {
        return new Event
        {
            Id = outboxEvent.Id,
            EventName = outboxEvent.EventName,
            Version = outboxEvent.Version,
            Payload = outboxEvent.Payload,
            CreatedAt = outboxEvent.CreatedAt,
            Source = null 
        };
    }
}