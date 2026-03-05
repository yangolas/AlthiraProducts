using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.Products.Application.Models.Persistence.Read;

namespace AlthiraProducts.Products.Application.Diagnostic.Telemetry.CategoryTelemetry;

internal static class GetCategoryMetadaContext
{
    internal static void AddCreateProductCommandHandlerMetadata(this IOpenTelemetryService telemetryService, IEnumerable<CategoryReadModel> categoryReadModel)
    {
        telemetryService.AddTag("query.result_count", categoryReadModel.Count());
    }
}