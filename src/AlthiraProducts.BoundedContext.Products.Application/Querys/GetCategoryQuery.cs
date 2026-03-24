using AlthiraProducts.BoundedContext.Products.Application.Models.Dtos;
using MediatR;

namespace AlthiraProducts.BoundedContext.Products.Application.Querys;

public record class GetCategoryQuery() : IRequest<IEnumerable<CategoryDto>>
{
}