using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.Products.Application.Models.Persistence.Read;
using static AlthiraProducts.BuildingBlocks.Application.Diagnostic.Telemetry.ConstantTag;

namespace AlthiraProducts.Products.Application.Diagnostic.Telemetry.CategoryTelemetry;

internal static class GetCategoryMetadaContext
{
    internal static void AddGetCategoryCommandHandlerMetadata(this IOpenTelemetryService telemetryService, IEnumerable<CategoryReadModel> categoryReadModel)
    {
        telemetryService.AddTag($"{MicroserviceName}.{EntityCategory}.{Count}", categoryReadModel.Count().ToString());
    }
}