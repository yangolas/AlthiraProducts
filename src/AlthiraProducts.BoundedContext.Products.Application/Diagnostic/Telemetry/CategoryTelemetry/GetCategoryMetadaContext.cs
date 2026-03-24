using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using static AlthiraProducts.BuildingBlocks.Application.Diagnostic.Telemetry.ConstantTag;

namespace AlthiraProducts.BoundedContext.Products.Application.Diagnostic.Telemetry.CategoryTelemetry;

internal static class GetCategoryMetadaContext
{
    internal static void AddGetCategoryCommandHandlerMetadata(this IOpenTelemetryService telemetryService, IEnumerable<CategoryReadModel> categoryReadModel)
    {
        telemetryService.AddTag($"{MicroserviceName}.{EntityCategory}.{Count}", categoryReadModel.Count().ToString());
    }
}