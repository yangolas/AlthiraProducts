using AlthiraProducts.Adapters.AzureBlobStorage.Ports;
using AlthiraProducts.Adapters.AzureBlobStorage.Ports.Extensions;
using AlthiraProducts.Adapters.Repository.Read.Models;
using AlthiraProducts.Products.Application.Dtos;
using AlthiraProducts.Products.Application.Dtos.ResponsesApi;
using AlthiraProducts.Products.Domain.Enums;
namespace AlthiraProducts.Products.Application.Mappers.Dtos;

public class Mapper_ProductRepositoryRead_ProductDto
{
    public static IEnumerable<ProductDto> MapToDto(
        IEnumerable<ProductReadModel> products, 
        IProductImageBlobStorageService productImageBlobStorageService)
    {
        return products.Select(product => new ProductDto
        {
            Sku = product.Sku,
            Name = product.Name,
            Price = product.Price,
            ProductStatus = (ProductStatus)product.Status,
            Category = new CategoryDto 
            { 
                Id = product.Category.Id, 
                Name = product.Category.Name 
            },
            Description = product.Description,
            Images = product.Images.Select(imgId =>
                productImageBlobStorageService.GetReadOnlySasUri($"{imgId}", isTemp: false))
        });
    }
}