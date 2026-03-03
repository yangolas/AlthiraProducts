using AlthiraProducts.Products.Application.Dtos.RequestsApi;
using AlthiraProducts.Products.Domain.Entities;
using AlthiraProducts.Products.Domain.ValueObject;
using Microsoft.AspNetCore.Http;

namespace AlthiraProducts.Products.Application.Mappers.Domain;

public static class Mapper_CreateProdcutDto_Product
{
    private static Stream ToStream(IFormFile file)
    {
        Stream ms = new MemoryStream();
        file.CopyTo(ms);
        return ms;
    }

    public static Product MapToEntity(this CreateProductDto createProductDto)
    {

        IEnumerable<ProductImage> productImages = createProductDto.Images
            .Select((file, index) => ProductImage.Create(
                index,
                ToStream(file),
                file.ContentType
            )).ToList() ?? []; ;

        return Product.Create (
            new Sku(Guid.NewGuid()),
            new Category( 
                createProductDto.Category.Id,
                createProductDto.Category.Name),
            createProductDto.ProductStatus,
            createProductDto.Name,
            createProductDto.Price,
            createProductDto.Description,
            productImages
            );
    }
}
