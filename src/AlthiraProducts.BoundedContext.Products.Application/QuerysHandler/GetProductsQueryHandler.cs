using AlthiraProducts.BoundedContext.Products.Application.Diagnostic.Telemetry.ProductTelemetry;
using AlthiraProducts.BoundedContext.Products.Application.Mappers.Dtos;
using AlthiraProducts.BoundedContext.Products.Application.Models.Dtos.ResponsesApi;
using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;
using AlthiraProducts.BoundedContext.Products.Application.Ports.AzureBlobStorage;
using AlthiraProducts.BoundedContext.Products.Application.Ports.RepositoryRead;
using AlthiraProducts.BoundedContext.Products.Application.Querys;
using AlthiraProducts.BuildingBlocks.Application.Models.Pagination;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.BoundedContext.Products.Application.QuerysHandler;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
{
    private readonly ILogger<GetProductsQueryHandler> _logger;
    private readonly IOpenTelemetryService _openTelemetryService;
    private readonly IProductRepositoryRead _productRepositoryRead;
    private readonly IProductImageBlobStorageService _productImageBlobStorageService;
    public GetProductsQueryHandler(
        ILogger<GetProductsQueryHandler> logger,
        IOpenTelemetryService openTelemetryService,
        IProductRepositoryRead productRepositoryRead,
        IProductImageBlobStorageService productImageBlobStorageService)
    {
        _logger = logger;
        _openTelemetryService = openTelemetryService;
        _productRepositoryRead = productRepositoryRead;
        _productImageBlobStorageService = productImageBlobStorageService;
    }

    public async Task<PagedResult<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        _openTelemetryService.AddStep("Fetching products from Read Repository");
        _openTelemetryService.AddGetProductCommandHandlerMetadata(request);

        (IEnumerable<ProductReadModel>,long) tuple = await _productRepositoryRead.GetProductsAsync(
            request.PagedRequest.Page,
            request.PagedRequest.PageSize,
            request.PagedRequest.OrderBy,
            request.PagedRequest.Descending
        );
        
        IEnumerable<ProductReadModel> productsReadRepository = tuple.Item1;
        long totalCount = tuple.Item2;

        PagedResult<ProductDto> pagedResult = new ()
        {
            Items = Mapper_ProductRepositoryRead_ProductDto.MapToDto(productsReadRepository, _productImageBlobStorageService),
            TotalCount = totalCount,
            Page = request.PagedRequest.Page,
            PageSize = request.PagedRequest.PageSize
        };
        
        _openTelemetryService.AddStep("Mapping results and resolving Blob Storage URLs");
        _logger.LogInformation("GetProductsQuery processed. Page: {Page}, Results: {Count}",
            request.PagedRequest.Page, 
            productsReadRepository.Count());

        return pagedResult;
    }
}
