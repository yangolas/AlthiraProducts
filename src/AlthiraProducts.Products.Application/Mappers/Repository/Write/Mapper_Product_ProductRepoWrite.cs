using AlthiraProducts.Products.Application.Models.Persistence.Write;
using AlthiraProducts.Products.Domain.Entities;

namespace AlthiraProducts.Products.Application.Mappers.Repository.Write;

public static class Mapper_Product_ProductRepoWrite
{
    public static ProductWriteModel MapToRepoWrite(this Product product)
    {
        return new ProductWriteModel()
        {
            Id = product.Id,
            Sku = product.Sku.Id,
            Version = product.Version,
            Name = product.Name,
            Price = product.Price,
            Status = (int)product.Status,
            CategoryId = product.Category.Id,
            Description = product.Description,
            Images = [.. product.ProductImages
            .Select((image) => ProductImageWriteModel.Create(
                image.Name,
                image.ContentType,
                image.Order,
                product.Id))]
        };
    }
}