using AlthiraProducts.Products.Application.Models.Dtos;
using AlthiraProducts.Products.Application.Models.Persistence.Read;

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