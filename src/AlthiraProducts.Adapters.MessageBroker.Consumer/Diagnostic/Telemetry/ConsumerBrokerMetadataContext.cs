using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.Diagnostic.Telemetry;

internal static class ConsumerBrokerMetadataContext
{
    internal static void AddConsumerBrokerMetadata(this IOpenTelemetryService telemetryService, Event @event)
    {
        telemetryService.AddTag("althiraproducts.message_broker.consumer.event_id", @event.Id);
        telemetryService.AddTag("althiraproducts.message_broker.consumer.event_name", @event.EventName);
        telemetryService.AddTag("althiraproducts.message_broker.consumer.event_version", @event.Version);
    }

    internal static void AddConsumerBrokerRetryMetadata(this IOpenTelemetryService telemetryService, int nextRetryLevel, string nextRetryRoutingKey)
    {
        telemetryService.AddTag("althiraproducts.message_broker.consumer.next_level", nextRetryLevel);
        telemetryService.AddTag("althiraproducts.message_broker.consumer.routing_key", nextRetryRoutingKey);
    }

    internal static void AddConsumerBrokerDLQMetadata(this IOpenTelemetryService telemetryService)
    {
        telemetryService.AddTag("althiraproducts.message_broker.consumer.status", "dead_letter");
    }
}
