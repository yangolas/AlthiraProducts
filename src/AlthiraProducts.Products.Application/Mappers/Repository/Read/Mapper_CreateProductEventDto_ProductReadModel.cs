using AlthiraProducts.Products.Application.Models.Dtos.Events;
using AlthiraProducts.Products.Application.Models.Persistence.Read;

namespace AlthiraProducts.Products.Application.Mappers.Repository.Read;

public static class Mapper_CreateProductEventDto_ProductReadModel
{
    public static ProductReadModel MapToRepoRead(this CreateProductEventDto createProductEventDto)
    {
        return new ProductReadModel()
        {
            Id = createProductEventDto.Id,
            Sku = createProductEventDto.Sku,
            Name = createProductEventDto.Name,
            Price = createProductEventDto.Price,
            Status = (int)createProductEventDto.ProductStatus,
            Category = new CategoryReadModel()
            {
                Id = createProductEventDto.Category.Id,
                Name = createProductEventDto.Category.Name
            },
            Description = createProductEventDto.Description,
            Images = createProductEventDto.Images
        };
    }
}
