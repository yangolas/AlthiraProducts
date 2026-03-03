using AlthiraProducts.Adapters.OpenTelemetry.Ports;
using AlthiraProducts.Products.Domain.Entities;

namespace AlthiraProducts.Products.Application.Diagnostic.Telemetry;

internal static class ProductMetadaContext
{
    internal static void AddProductMetadata(this IOpenTelemetryService telemetryService, Product product)
    {
        telemetryService.AddTag("althiraproduct.product.id", product.Id);
        telemetryService.AddTag("althiraproduct.product.sku", product.Sku.Id);
        telemetryService.AddTag("althiraproduct.product.category_id", product.Category.Id);
        telemetryService.AddTag("althiraproduct.product.image_name", product.ProductImages.Select(i => i.Name));
    }
}
