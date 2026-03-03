using AlthiraProducts.Adapters.Repository.Read.Models;
using AlthiraProducts.Adapters.Repository.Read.Ports;
using AlthiraProducts.Products.Application.Dtos;
using AlthiraProducts.Products.Application.Mappers.Dtos;
using AlthiraProducts.Products.Application.Querys;
using MediatR;

namespace AlthiraProducts.Products.Application.QuerysHandler;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepositoryRead _categoryRepositoryRead;

    public GetCategoryQueryHandler(ICategoryRepositoryRead categoryRepositoryRead)
    {
        _categoryRepositoryRead = categoryRepositoryRead;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<CategoryReadModel> categoriesReadModel = await _categoryRepositoryRead.GetCategoriesAsync();

        IEnumerable<CategoryDto> categoriesDtos = Mapper_CategoryReadModel_CategoryDto.MapToDto(categoriesReadModel);

        return categoriesDtos;
    }
}