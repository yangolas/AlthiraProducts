using AlthiraProducts.BoundedContext.Products.Application.Models.Dtos.RequestsApi;
using MediatR;

namespace AlthiraProducts.BoundedContext.Products.Application.Commands;

public class CreateProductCommand(CreateProductDto createProductDto) :IRequest<Guid>
{
    public CreateProductDto CreateProductDto { get; set; } = createProductDto;
}
