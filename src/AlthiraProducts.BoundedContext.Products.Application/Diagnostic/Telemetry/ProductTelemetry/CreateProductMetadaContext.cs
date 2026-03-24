using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;
using AlthiraProducts.BoundedContext.Products.Domain.Entities;
using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using static AlthiraProducts.BuildingBlocks.Application.Diagnostic.Telemetry.ConstantTag;

namespace AlthiraProducts.BoundedContext.Products.Application.Diagnostic.Telemetry.ProductTelemetry;

internal static class CreateProductMetadaContext
{
    internal static void AddCreateProductCommandHandlerMetadata(this IOpenTelemetryService telemetryService, Product product, Event @event)
    {
        telemetryService.AddTag($"{MicroserviceName}.{EntityProduct}.{Id}", product.Id.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityProduct}.{Sku}", product.Sku.Id.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityProduct}.{EntityCategory}.{Id}", product.Category.Id.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityProduct}.{EntityImage}.{Name}", product.ProductImages.Select(i => i.Name.ToString()));
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Id}", @event.Id.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Name}", @event.EventName);
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{CreatedAt}", @event.CreatedAt.ToString("o"));
    }

    internal static void AddCreateProductEventHandlerMetadata(this IOpenTelemetryService telemetryService, ProductReadModel product, Event @event)
    {
        telemetryService.AddTag($"{MicroserviceName}.{EntityProduct}.{Id}", product.Id.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{Id}", @event.Id.ToString());
        telemetryService.AddTag($"{MicroserviceName}.{EntityEvent}.{CreatedAt}", @event.CreatedAt.ToString("o"));
    }
}
