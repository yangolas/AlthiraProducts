using AlthiraProducts.BoundedContext.Products.Application.Ports.AzureBlobStorage;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.AzureBlobStorage.Services.Extensions;

public class ProductImageBlobStorageService(
    ILogger<ProductImageBlobStorageService> logger,
    string connectionString,
    string containerName,
    IngressLocalKubernetesSettings? ingressLocalKubernetesSettings) :
        AzureBlobStorageService(
            logger,
            connectionString,
            containerName,
            ingressLocalKubernetesSettings), IProductImageBlobStorageService
{
}