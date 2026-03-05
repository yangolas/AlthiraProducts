using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.Products.Application.Models.Persistence.Read;
using AlthiraProducts.Products.Domain.Entities;

namespace AlthiraProducts.Products.Application.Diagnostic.Telemetry.ProductTelemetry;

internal static class CreateProductMetadaContext
{
    internal static void AddCreateProductCommandHandlerMetadata(this IOpenTelemetryService telemetryService, Product product)
    {
        telemetryService.AddTag("althiraproduct.product.id", product.Id);
        telemetryService.AddTag("althiraproduct.product.sku", product.Sku.Id);
        telemetryService.AddTag("althiraproduct.product.category_id", product.Category.Id);
        telemetryService.AddTag("althiraproduct.product.image_name", product.ProductImages.Select(i => i.Name));
    }

    internal static void AddCreateProductEventHandlerMetadata(this IOpenTelemetryService telemetryService, ProductReadModel product, Event @event)
    {
        telemetryService.AddTag("product.id", product.Id.ToString());
        telemetryService.AddTag("event.id", @event.Id.ToString());
    }
}
