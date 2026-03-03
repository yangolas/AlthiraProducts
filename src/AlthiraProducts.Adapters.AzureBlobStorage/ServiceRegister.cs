using AlthiraProducts.Adapters.AzureBlobStorage.Ports.Extensions;
using AlthiraProducts.Adapters.AzureBlobStorage.Services.Extensions;
using AlthiraProducts.Main.Settings.Models;
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
            string connectionString = azureBlobStorageSettings.ConnectionString;
            string containerName = azureBlobStorageSettings.ProductImageBlobContainer.Name;

            return new ProductImageBlobStorageService(logger, connectionString, containerName);
        });
    }
}