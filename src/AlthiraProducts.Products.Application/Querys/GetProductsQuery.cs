using AlthiraProducts.BuildingBlocks.Application.Models.Pagination;
using AlthiraProducts.Products.Application.Models.Dtos.ResponsesApi;
using MediatR;

namespace AlthiraProducts.Products.Application.Querys;

public record class GetProductsQuery(PagedRequest pagedRequest) : IRequest<PagedResult<ProductDto>>
{
    public PagedRequest PagedRequest{ get; set; } = pagedRequest;
}