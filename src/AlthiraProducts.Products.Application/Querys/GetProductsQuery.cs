using AlthiraProducts.BuildingBlocks.Aplication.Pagination;
using AlthiraProducts.Products.Application.Dtos.ResponsesApi;
using MediatR;

namespace AlthiraProducts.Products.Application.Querys;

public record class GetProductsQuery(PagedRequest pagedRequest) : IRequest<PagedResult<ProductDto>>
{
    public PagedRequest PagedRequest{ get; set; } = pagedRequest;
}