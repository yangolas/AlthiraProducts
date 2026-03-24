using AlthiraProducts.BuildingBlocks.Application.EventModel;

namespace AlthiraProducts.Products.Application.Mappers.Events;

internal static class MapperBaseEvents
{
    internal static Event MapToEvent(
        Event @event,
        string eventTypeName,
        int version,
        string source,
        string payload)
    {
        @event.Id = Guid.NewGuid();
        @event.EventName = eventTypeName;
        @event.Version = version;
        @event.Payload = payload;
        @event.CreatedAt = DateTime.UtcNow;
        @event.Source = source;
        return @event;
    }
}