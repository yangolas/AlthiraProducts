using AlthiraProducts.Adapters.AzureBlobStorage.Services.Extensions;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using AlthiraProducts.Products.Application.Ports.AzureBlobStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.AzureBlobStorage;

public static class ServiceRegister
{
    public static void AddAzureBlobStorageService(this IServiceCollection services, AzureBlobStorageSettings azureBlobStorageSettings)
    {
        services.AddSingleton<IProductImageBlobStorageService>(sp =>
        {
            ILogger<ProductImageBlobStorageService> logger = sp.GetRequiredService<ILogger<ProductImageBlobStorageService>>();
            IngressLocalKubernetesSettings? ingressLocalKubernetesSettings = azureBlobStorageSettings.IngressLocalKubernetes;
            string connectionString = azureBlobStorageSettings.ConnectionString;
            string containerName = azureBlobStorageSettings.ProductImageBlobContainer.Name;

            return new ProductImageBlobStorageService(logger, connectionString, containerName, ingressLocalKubernetesSettings);
        });
    }
}