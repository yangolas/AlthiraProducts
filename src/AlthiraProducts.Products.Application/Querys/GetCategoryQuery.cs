using AlthiraProducts.Products.Application.Dtos;
using MediatR;

namespace AlthiraProducts.Products.Application.Querys;

public record class GetCategoryQuery() : IRequest<IEnumerable<CategoryDto>>
{
}