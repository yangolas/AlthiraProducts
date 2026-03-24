using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;
using AlthiraProducts.BuildingBlocks.Application.EventModel;

namespace AlthiraProducts.BoundedContext.Products.Application.Mappers.Repository.Read;

public static class Mapper_Event_EventLogReadModel
{
    public static EventLogReadModel MapToRepoRead(this Event @event)
    {
        return new EventLogReadModel()
        {
            Id = @event.Id,
            EventName = @event.EventName,
            CreatedAt = @event.CreatedAt,
            Source = @event.Source,
        };
    }
}