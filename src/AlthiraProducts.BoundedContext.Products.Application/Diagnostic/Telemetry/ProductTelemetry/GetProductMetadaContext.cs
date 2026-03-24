using AlthiraProducts.BoundedContext.Products.Application.Querys;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;

namespace AlthiraProducts.BoundedContext.Products.Application.Diagnostic.Telemetry.ProductTelemetry;

internal static class GetProductMetadaContext
{
    internal static void AddGetProductCommandHandlerMetadata(this IOpenTelemetryService telemetryService, GetProductsQuery request)
    {
        telemetryService.AddTag("query.page", request.PagedRequest.Page);
        telemetryService.AddTag("query.page_size", request.PagedRequest.PageSize);
        telemetryService.AddTag("query.order_by", request.PagedRequest.OrderBy ?? "None");
    }
}

