using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.Diagnostic.Telemetry;

internal static class PublisherMetadaContext
{
    internal static void AddPublisherBrokerMetadata(this IOpenTelemetryService telemetryService, Event @event, string routingKey)
    {
        telemetryService.AddTag("messaging.destination", routingKey);
        telemetryService.AddTag("messaging.event_id", @event.Id);
    }
}
