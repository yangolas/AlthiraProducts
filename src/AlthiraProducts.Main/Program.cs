using AlthiraProducts.Adapters.AzureBlobStorage;
using AlthiraProducts.Adapters.AzureBlobStorageProcess;
using AlthiraProducts.Adapters.AzureBlobStorageProcess.HostInfraestructure;
using AlthiraProducts.Adapters.ImagesValidator;
using AlthiraProducts.Adapters.MessageBroker.Consumer.HostInfraestructure;
using AlthiraProducts.Adapters.MessageBroker.Consumer.ServicesRegister;
using AlthiraProducts.Adapters.MessageBroker.Publisher.ServicesRegister;
using AlthiraProducts.Adapters.OpenTelemetry;
using AlthiraProducts.Adapters.Outbox;
using AlthiraProducts.Adapters.Outbox.HostInfraestructure;
using AlthiraProducts.Adapters.Repository.Read.Services;
using AlthiraProducts.Adapters.Repository.Write;
using AlthiraProducts.Adapters.WebApi;
using AlthiraProducts.Products.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

var cultureInfo = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

IConfigurationRoot configurationRoot = builder.Build();
IConfigurationSection configurationSection = configurationRoot.GetSection("Config");
AlthiraProductsSettings config = configurationSection.Get<AlthiraProductsSettings>()!;

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

IServiceCollection servicesConsumerBrokerMessage = new ServiceCollection();
servicesConsumerBrokerMessage.Configure<AlthiraProductsSettings>(configurationSection);
servicesConsumerBrokerMessage.AddOpenTelemetry(config.OpenTelemetry.ConsumerBrokerName, config.OpenTelemetry.ConnectionString);
servicesConsumerBrokerMessage.AddApplicationProduct(config.Assembly.AssemblyApplicationProduct);
servicesConsumerBrokerMessage.AddHostedConsumerBroker(config.MessageBroker, config.Events);
servicesConsumerBrokerMessage.AddRepositoryContextRead(config.DatabaseRead);
servicesConsumerBrokerMessage.AddRepositoryRead(config.Assembly.AssemblyRepositoryRead);

IServiceCollection servicesOutboxWorker = new ServiceCollection();
servicesOutboxWorker.Configure<AlthiraProductsSettings>(configurationSection);
servicesOutboxWorker.AddOpenTelemetry(config.OpenTelemetry.OutboxWorkerName, config.OpenTelemetry.ConnectionString);
servicesOutboxWorker.AddRepositoryContextWrite(config.DatabaseWrite);
servicesOutboxWorker.AddRepositoryWrite(config.Assembly.AssemblyRepositoryWrite);
servicesOutboxWorker.AddOutboxPublisherBroker(config.MessageBroker, config.Events);
servicesOutboxWorker.AddOutboxWorker();

IServiceCollection servicesAzureStorageBlobWorker = new ServiceCollection();
servicesAzureStorageBlobWorker.Configure<AlthiraProductsSettings>(configurationSection);
servicesAzureStorageBlobWorker.AddOpenTelemetry(config.OpenTelemetry.AzureBlobStorageWorkerName, config.OpenTelemetry.ConnectionString);
servicesAzureStorageBlobWorker.AddAzureBlobStorageService(config.AzureBlobStorage);
servicesAzureStorageBlobWorker.AddAzureBlobStorageProcess(config.AzureBlobStorage);
servicesAzureStorageBlobWorker.AddRepositoryContextWrite(config.DatabaseWrite);
servicesAzureStorageBlobWorker.AddRepositoryWrite(config.Assembly.AssemblyRepositoryWrite);


Task webApiTask = webApi.StartAsync();
Task consumerBrokerTask = StartConsumerBrokerWorker.Main(servicesConsumerBrokerMessage);// One Worker per Bounded Context, N Consumers per Bounded Context 
Task outboxTask = StartOutboxWorker.Main(servicesOutboxWorker);//One worker per db context
Task azureBlobStorageTask = StartAzureBloStorageWorker.Main(servicesAzureStorageBlobWorker);//One worker per db context

await Task.WhenAll(
    webApiTask, 
    consumerBrokerTask,
    outboxTask,
    azureBlobStorageTask);