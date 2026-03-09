using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.Products.Application.Models.Persistence.Write;

namespace AlthiraProducts.Products.Application.Mappers.Repository.Write;

public static class Mapper_Event_OutboxEventWriteModel
{
    public static OutboxEventWriteModel MapToOutboxEvent(this Event @event, string traceContext)
    {
        return OutboxEventWriteModel.Create(
            @event.EventName,
            @event.Version,
            @event.Payload,
            @event.CreatedAt,
            traceContext);
    }
}