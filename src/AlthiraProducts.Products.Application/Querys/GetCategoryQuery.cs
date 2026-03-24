using AlthiraProducts.Products.Application.Models.Dtos;
using MediatR;

namespace AlthiraProducts.Products.Application.Querys;

public record class GetCategoryQuery() : IRequest<IEnumerable<CategoryDto>>
{
}