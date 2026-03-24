using AlthiraProducts.BoundedContext.Products.Application.Models.Dtos;
using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;

namespace AlthiraProducts.BoundedContext.Products.Application.Mappers.Dtos;

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