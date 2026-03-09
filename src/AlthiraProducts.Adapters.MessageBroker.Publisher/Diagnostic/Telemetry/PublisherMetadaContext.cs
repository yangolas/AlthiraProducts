using AlthiraProducts.BuildingBlocks.Application.Diagnostic.Telemetry;
using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using RabbitMQ.Client;
using static AlthiraProducts.BuildingBlocks.Application.Diagnostic.Telemetry.ConstantTag;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.Diagnostic.Telemetry;

internal static class PublisherMetadaContext
{
    internal static void AddPublisherBrokerMetadata(this IOpenTelemetryService telemetryService, Event @event, string routingKey, string exchange, AmqpTimestamp inQueueAt)
    {

        long unixTime = inQueueAt.UnixTime;
        string date = DateTimeOffset.FromUnixTimeSeconds(unixTime).ToString("o");

        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Id}", @event.Id.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Name}", @event.EventName);
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{ConstantTag.Version}", @event.Version.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{CreatedAt}", @event.CreatedAt.ToString("o"));
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{RoutingKey}", routingKey);
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{Exchange}", exchange);
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{InQueueAt}", date);
    }
}
