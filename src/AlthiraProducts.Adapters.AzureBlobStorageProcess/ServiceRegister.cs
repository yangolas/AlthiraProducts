using AlthiraProducts.Adapters.AzureBlobStorageProcess.HostInfraestructure;
using AlthiraProducts.Adapters.AzureBlobStorageProcess.Services.Extensions;
using AlthiraProducts.BoundedContext.Products.Application.Ports.AzureBlobSotageProcess;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.AzureBlobStorageProcess;

public static class ServiceRegister
{
    public static void AddAzureBlobStorageProcess(
        this IServiceCollection services)
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