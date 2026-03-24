using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Write;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using static AlthiraProducts.BuildingBlocks.Application.Diagnostic.Telemetry.ConstantTag;

namespace AlthiraProducts.Adapters.AzureBlobStorageProcess.Diagnostic;

internal static class AzureBlobStorageMetada
{
    internal static void AddProductBlobStorageMetada(this IOpenTelemetryService telemetryService, ProductImageWriteModel productImageWriteModel) 
    {
        telemetryService.AddTag($"{MicroserviceName}.{EntityImage}.{Name}", productImageWriteModel.Name.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityImage}.{Retry}", productImageWriteModel.RetryCount.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityProductBlobStorage}.{CreatedAt}", productImageWriteModel.InsertedAt.ToString("o"));
        telemetryService.AddTag($"{MicroserviceName}.{EntityProductBlobStorage}.{Retry}", productImageWriteModel.RetryCount.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityProductBlobStorage}.{Status}", productImageWriteModel.Status.ToString());
    }
}
