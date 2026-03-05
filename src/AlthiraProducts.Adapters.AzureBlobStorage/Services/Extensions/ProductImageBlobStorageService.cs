using AlthiraProducts.Products.Application.Ports.AzureBlobStorage;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.AzureBlobStorage.Services.Extensions;

public class ProductImageBlobStorageService(
    ILogger<ProductImageBlobStorageService> logger,
    string connectionString,
    string containerName) : AzureBlobStorageService(logger, connectionString, containerName), IProductImageBlobStorageService
{
}