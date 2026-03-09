using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.Products.Application.Diagnostic.Telemetry.CategoryTelemetry;
using AlthiraProducts.Products.Application.Mappers.Dtos;
using AlthiraProducts.Products.Application.Models.Dtos;
using AlthiraProducts.Products.Application.Models.Persistence.Read;
using AlthiraProducts.Products.Application.Ports.RepositoryRead;
using AlthiraProducts.Products.Application.Querys;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Products.Application.QuerysHandler;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, IEnumerable<CategoryDto>>
{
    private readonly ILogger<GetCategoryQueryHandler> _logger;
    private readonly IOpenTelemetryService _openTelemetryService;
    private readonly ICategoryRepositoryRead _categoryRepositoryRead;

    public GetCategoryQueryHandler(
        ILogger<GetCategoryQueryHandler> logger,
        IOpenTelemetryService openTelemetryService,
        ICategoryRepositoryRead categoryRepositoryRead)
    {
        _logger = logger;
        _openTelemetryService = openTelemetryService;
        _categoryRepositoryRead = categoryRepositoryRead;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        _openTelemetryService.AddStep("Fetching categories from Read Repository");
        IEnumerable<CategoryReadModel> categoriesReadModel = await _categoryRepositoryRead.GetCategoriesAsync();

        _openTelemetryService.AddGetCategoryCommandHandlerMetadata(categoriesReadModel);
        IEnumerable<CategoryDto> categoriesDtos = Mapper_CategoryReadModel_CategoryDto.MapToDto(categoriesReadModel);

        _logger.LogInformation("GetCategoryQuery executed successfully. Categories found: {Count}",
            categoriesReadModel.Count());

        _openTelemetryService.AddStep("Returning categories list");

        return categoriesDtos;
    }
}