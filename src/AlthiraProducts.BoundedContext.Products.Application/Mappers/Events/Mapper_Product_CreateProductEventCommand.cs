using AlthiraProducts.BoundedContext.Products.Application.Models.Dtos;
using AlthiraProducts.BoundedContext.Products.Application.Models.Dtos.Events;
using AlthiraProducts.BoundedContext.Products.Application.Models.Events;
using AlthiraProducts.BoundedContext.Products.Domain.Entities;
using System.Text.Json;

namespace AlthiraProducts.BoundedContext.Products.Application.Mappers.Events;

internal static class Mapper_Product_CreateProductEventCommand
{
    internal static CreateProductEventCommand MapToEvent(this Product product, string source)
    {
        
        return (CreateProductEventCommand)MapperBaseEvents.MapToEvent(
            new CreateProductEventCommand(),
            typeof(CreateProductEventCommand).Name,
            product.Version,
            source,
            JsonSerializer.Serialize(new CreateProductEventDto 
            { 
                Id = product.Id,
                Sku = product.Sku.Id,
                Name = product.Name,
                Price = product.Price,
                ProductStatus = product.Status,
                Category = new CategoryDto
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name,
                },
                Description = product.Description,
                Images = product.ProductImages.Select(image => image.Name.ToString()),
            })); 
    }
}