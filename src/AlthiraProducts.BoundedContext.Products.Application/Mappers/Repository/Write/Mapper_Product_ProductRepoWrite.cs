using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Write;
using AlthiraProducts.BoundedContext.Products.Domain.Entities;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using System.Text.Json;

namespace AlthiraProducts.BoundedContext.Products.Application.Mappers.Repository.Write;

public static class Mapper_Product_ProductRepoWrite
{
    public static ProductWriteModel MapToRepoWrite(this Product product, string traceContext)
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
                product.Id,
                traceContext))]
        };
    }
}