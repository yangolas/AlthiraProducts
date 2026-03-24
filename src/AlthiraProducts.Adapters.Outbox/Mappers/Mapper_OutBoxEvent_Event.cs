using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Write;
using AlthiraProducts.BuildingBlocks.Application.EventModel;

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
            CreatedAt = outboxEvent.InsertedAt,
            Source = null 
        };
    }
}