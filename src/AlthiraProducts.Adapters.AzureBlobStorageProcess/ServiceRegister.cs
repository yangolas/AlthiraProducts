using AlthiraProducts.Adapters.AzureBlobStorageProcess.HostInfraestructure;
using AlthiraProducts.Adapters.AzureBlobStorageProcess.Ports;
using AlthiraProducts.Adapters.AzureBlobStorageProcess.Services.Extensions;
using AlthiraProducts.Adapters.OpenTelemetry.Ports;
using AlthiraProducts.Main.Settings.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.AzureBlobStorageProcess;

public static class ServiceRegister
{
    public static void AddAzureBlobStorageProcess(
        this IServiceCollection services,
        AzureBlobStorageSettings azureBlobStorageSettings)
    {
        services.AddSingleton<IProductImageBlobStorageProcess, ProductImageBlobStorageProcess>();
        services.AddHostedService(sp => 
        {
            ILogger<AzureBloStorageWorker> logger = sp.GetRequiredService<ILogger<AzureBloStorageWorker>>();
            IOpenTelemetryService openTelemetryService = sp.GetRequiredService<IOpenTelemetryService>();
            IProductImageBlobStorageProcess ProductImageBlobStorageProcess = sp.GetRequiredService<IProductImageBlobStorageProcess>();
            return new AzureBloStorageWorker(logger, openTelemetryService, ProductImageBlobStorageProcess);
        });
    }
}