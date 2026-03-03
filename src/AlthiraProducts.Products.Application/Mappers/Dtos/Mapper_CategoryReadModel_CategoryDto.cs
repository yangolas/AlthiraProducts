using AlthiraProducts.Adapters.Repository.Read.Models;
using AlthiraProducts.Products.Application.Dtos;
namespace AlthiraProducts.Products.Application.Mappers.Dtos;

public class Mapper_CategoryReadModel_CategoryDto
{
    public static IEnumerable<CategoryDto> MapToDto(IEnumerable<CategoryReadModel> categories)
    {
        return categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
        });
    }
}