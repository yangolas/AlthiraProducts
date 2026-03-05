using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.Products.Application.Models.Persistence.Write;

namespace AlthiraProducts.Adapters.AzureBlobStorageProcess.Diagnostic;

internal static class AzureBlobStorageMetada
{
    internal static void AddProductBlobStorageMetada(this IOpenTelemetryService telemetryService, ProductImageWriteModel productImageWriteModel) 
    {
        telemetryService.AddTag("althiraproducts.product_blob_storage.event_id", productImageWriteModel.Name);
        telemetryService.AddTag("althiraproducts.product_blob_storage.retry_count", productImageWriteModel.RetryCount);
    }
}
