using AlthiraProducts.Adapters.AzureBlobStorage;
using AlthiraProducts.Adapters.AzureBlobStorageProcess;
using AlthiraProducts.Adapters.AzureBlobStorageProcess.HostInfraestructure;
using AlthiraProducts.Adapters.OpenTelemetry;
using AlthiraProducts.Adapters.Repository.Write;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlthiraProducts.Main.Process;

internal static class ImageProcess
{
    internal static Task Start(IConfigurationSection configurationSection, AlthiraProductsSettings config)
    {
        IServiceCollection servicesAzureStorageBlobWorker = new ServiceCollection();
        servicesAzureStorageBlobWorker.Configure<AzureBlobStorageSettings>(configurationSection.GetSection("AzureBlobStorage"));
        servicesAzureStorageBlobWorker.AddOpenTelemetry(config.OpenTelemetry.AzureBlobStorageWorkerName, config.OpenTelemetry.ConnectionString);
        servicesAzureStorageBlobWorker.AddAzureBlobStorageService(config.AzureBlobStorage);
        servicesAzureStorageBlobWorker.AddAzureBlobStorageProcess();
        servicesAzureStorageBlobWorker.AddRepositoryContextWrite(config.DatabaseWrite);
        servicesAzureStorageBlobWorker.AddRepositoryWrite(config.Assembly.AssemblyRepositoryWrite);


        Task azureBlobStorageTask = StartAzureBloStorageWorker.Main(servicesAzureStorageBlobWorker);//One worker per db context
        return azureBlobStorageTask;
    }
}