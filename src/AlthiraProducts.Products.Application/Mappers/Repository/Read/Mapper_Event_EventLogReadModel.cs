using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.Products.Application.Models.Persistence.Read;

namespace AlthiraProducts.Products.Application.Mappers.Repository.Read;

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