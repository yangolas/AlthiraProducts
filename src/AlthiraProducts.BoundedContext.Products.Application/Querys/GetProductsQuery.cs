using AlthiraProducts.BoundedContext.Products.Application.Models.Dtos.ResponsesApi;
using AlthiraProducts.BuildingBlocks.Application.Models.Pagination;
using MediatR;

namespace AlthiraProducts.BoundedContext.Products.Application.Querys;

public record class GetProductsQuery(PagedRequest pagedRequest) : IRequest<PagedResult<ProductDto>>
{
    public PagedRequest PagedRequest{ get; set; } = pagedRequest;
}