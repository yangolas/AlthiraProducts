using AlthiraProducts.Adapters.MessageBroker.Events.Models;
using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;

namespace AlthiraProducts.Products.Application.Mappers.Repository.Write;

public static class Mapper_Event_OutboxEventWriteModel
{
    public static OutboxEventWriteModel MapToOutboxEvent(this Event @event)
    {
        return OutboxEventWriteModel.Create(
            @event.EventName,
            @event.Version,
            @event.Payload);
    }
}