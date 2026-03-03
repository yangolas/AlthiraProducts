using AlthiraProducts.Adapters.AzureBlobStorage.Ports.Extensions;
using AlthiraProducts.Adapters.Repository.Read.Models;
using AlthiraProducts.Adapters.Repository.Read.Ports;
using AlthiraProducts.BuildingBlocks.Aplication.Pagination;
using AlthiraProducts.Products.Application.Dtos.ResponsesApi;
using AlthiraProducts.Products.Application.Mappers.Dtos;
using AlthiraProducts.Products.Application.Querys;
using MediatR;

namespace AlthiraProducts.Products.Application.QuerysHandler;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
{
    private readonly IProductRepositoryRead _productRepositoryRead;
    private readonly IProductImageBlobStorageService _productImageBlobStorageService;
    public GetProductsQueryHandler(
        IProductRepositoryRead productRepositoryRead,
        IProductImageBlobStorageService productImageBlobStorageService)
    {
        _productRepositoryRead = productRepositoryRead;
        _productImageBlobStorageService = productImageBlobStorageService;
    }

    public async Task<PagedResult<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        (IEnumerable<ProductReadModel>,long) tuple = await _productRepositoryRead.GetProductsAsync(
            request.PagedRequest.Page,
            request.PagedRequest.PageSize,
            request.PagedRequest.OrderBy,
            request.PagedRequest.Descending
        );
        
        IEnumerable<ProductReadModel> productsReadRepository = tuple.Item1;
        long totalCount = tuple.Item2;

        return new PagedResult<ProductDto>
        {
            Items = Mapper_ProductRepositoryRead_ProductDto.MapToDto(productsReadRepository, _productImageBlobStorageService),
            TotalCount = totalCount,
            Page = request.PagedRequest.Page,
            PageSize = request.PagedRequest.PageSize
        };
    }
}
