using AlthiraProducts.BuildingBlocks.Application.Diagnostic.Telemetry;
using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using RabbitMQ.Client.Events;
using static AlthiraProducts.BuildingBlocks.Application.Diagnostic.Telemetry.ConstantTag;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.Diagnostic.Telemetry;

internal static class ConsumerBrokerMetadataContext
{
    internal static void AddConsumerBrokerMetadata(this IOpenTelemetryService telemetryService, Event @event)
    {
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Id}", @event.Id.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Name}", @event.EventName);
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{ConstantTag.Version}", @event.Version.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{CreatedAt}", @event.CreatedAt.ToString("o"));
    }
    internal static void AddConsumerBrokerMetadata(this IOpenTelemetryService telemetryService, Event eventBase, BasicDeliverEventArgs @event)
    {
        if (@event.BasicProperties.IsTimestampPresent())
        {
            long unixTime = @event.BasicProperties.Timestamp.UnixTime;
            string date = DateTimeOffset.FromUnixTimeSeconds(unixTime).ToString("o");
            telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{InQueueAt}", date);
        }
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Id}", eventBase.Id.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Name}", eventBase.EventName);
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{ConstantTag.Version}", eventBase.Version.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{CreatedAt}", eventBase.CreatedAt.ToString("o"));
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{Exchange}", @event.Exchange);
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{RoutingKey}", @event.RoutingKey);
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{Status}", "Ok");
    }

    internal static void AddConsumerBrokerRetryMetadata(this IOpenTelemetryService telemetryService, BasicDeliverEventArgs @event, int nextRetryLevel, string nextRetryRoutingKey)
    {
        if (@event.BasicProperties.IsTimestampPresent())
        {
            long unixTime = @event.BasicProperties.Timestamp.UnixTime;
            string date = DateTimeOffset.FromUnixTimeSeconds(unixTime).ToString("o");
            telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{InQueueAt}", date);
        }
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Id}", @event.BasicProperties.MessageId ?? "Uknow-Id");
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{Exchange}", @event.Exchange);
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{RoutingKey}", @event.RoutingKey);
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{Retry}", (nextRetryLevel - 1).ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{NextRetry}", nextRetryLevel.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{NextRoutingKey}", nextRetryRoutingKey);
    }

    internal static void AddConsumerBrokerDLQMetadata(this IOpenTelemetryService telemetryService, BasicDeliverEventArgs @event, int maxRetryAttempt)
    {
        if (@event.BasicProperties.IsTimestampPresent())
        {
            long unixTime = @event.BasicProperties.Timestamp.UnixTime;
            string date = DateTimeOffset.FromUnixTimeSeconds(unixTime).ToString("o");
            telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{InQueueAt}", date);
        }
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Id}", @event.BasicProperties.MessageId ?? "Uknow-Id");
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{Exchange}", @event.Exchange);
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{RoutingKey}", @event.RoutingKey);
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{MaxRetry}", maxRetryAttempt.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityMessageBroker}.{Status}", "Ko");
    }
}
