using AlthiraProducts.Products.Application.Models.Dtos.RequestsApi;
using MediatR;

namespace AlthiraProducts.Products.Application.Commands;

public class CreateProductCommand(CreateProductDto createProductDto) :IRequest<Guid>
{
    public CreateProductDto CreateProductDto { get; set; } = createProductDto;
}
