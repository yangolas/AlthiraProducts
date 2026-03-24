using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Write;
using AlthiraProducts.BuildingBlocks.Application.EventModel;

namespace AlthiraProducts.BoundedContext.Products.Application.Mappers.Repository.Write;

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