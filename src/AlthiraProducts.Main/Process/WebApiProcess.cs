using AlthiraProducts.Adapters.AzureBlobStorage;
using AlthiraProducts.Adapters.ImagesValidator;
using AlthiraProducts.Adapters.MessageBroker.Publisher.ServicesRegister;
using AlthiraProducts.Adapters.OpenTelemetry;
using AlthiraProducts.Adapters.Repository.Read.Services;
using AlthiraProducts.Adapters.Repository.Write;
using AlthiraProducts.Adapters.WebApi;
using AlthiraProducts.Products.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlthiraProducts.Main.Process;

internal static class WebApiProcess
{
    internal static Task Start(IConfigurationSection configurationSection, AlthiraProductsSettings config) 
    {
        IServiceCollection servicesApi = new ServiceCollection();
        servicesApi.Configure<AlthiraProductsSettings>(configurationSection);
        servicesApi.AddOpenTelemetry(config.OpenTelemetry.WebApiName, config.OpenTelemetry.ConnectionString);
        servicesApi.AddPublisherBroker(config.MessageBroker, config.Events);//One publisher per Bounde context, N events contains per Bounded Context
        servicesApi.AddRepositoryContextRead(config.DatabaseRead);
        servicesApi.AddRepositoryRead(config.Assembly.AssemblyRepositoryRead);
        servicesApi.AddRepositoryContextWrite(config.DatabaseWrite);
        servicesApi.AddImageValidator(config.Assembly.AssemblyImageValidator);
        servicesApi.AddRepositoryWrite(config.Assembly.AssemblyRepositoryWrite);
        servicesApi.AddAzureBlobStorageService(config.AzureBlobStorage);
        servicesApi.AddApplicationProduct(config.Assembly.AssemblyApplicationProduct);

        WebApi webApi = new();
        webApi.Configure(servicesApi);
        Task webApiTask = webApi.Start();
        return webApiTask;
    }
}