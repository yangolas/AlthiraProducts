using AlthiraProducts.Adapters.OpenTelemetry.Ports;
using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;

namespace AlthiraProducts.Adapters.Outbox.Diagnostic.Telemetry;

internal static class OtuboxMetadataContext
{
    internal static void AddOutboxMetadata(this IOpenTelemetryService telemetryService, OutboxEventWriteModel outboxEventWriteModel)
    {
        telemetryService.AddTag("althiraproducts.outbox.event_id", outboxEventWriteModel.Id);
        telemetryService.AddTag("althiraproducts.outbox.event_type", outboxEventWriteModel.EventName);
        telemetryService.AddTag("althiraproducts.outbox.retry_count", outboxEventWriteModel.RetryCount);
    }
}