using AlthiraProducts.BuildingBlocks.Application.Diagnostic.Telemetry;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.Products.Application.Models.Persistence.Write;
using static AlthiraProducts.BuildingBlocks.Application.Diagnostic.Telemetry.ConstantTag;

namespace AlthiraProducts.Adapters.Outbox.Diagnostic.Telemetry;

internal static class OtuboxMetadataContext
{
    internal static void AddOutboxMetadata(this IOpenTelemetryService telemetryService, OutboxEventWriteModel outboxEventWriteModel)
    {
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Id}", outboxEventWriteModel.Id.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Name}", outboxEventWriteModel.EventName);
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{ConstantTag.Version}", outboxEventWriteModel.Version.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{CreatedAt}", outboxEventWriteModel.CreatedAt.ToString("o"));
        telemetryService.AddTag($"{MicroserviceName}.{EntityOutbox}.{CreatedAt}", outboxEventWriteModel.InsertedAt.ToString("o"));
        telemetryService.AddTag($"{MicroserviceName}.{EntityOutbox}.{Retry}", outboxEventWriteModel.RetryCount.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityOutbox}.{Status}", outboxEventWriteModel.Status.ToString());
        if (outboxEventWriteModel.ProcessedAt.HasValue)
        {
            telemetryService.AddTag($"{MicroserviceName}.{EntityOutbox}.{ProcessedAt}", outboxEventWriteModel.ProcessedAt.Value.ToString("o"));
        }
    }
}