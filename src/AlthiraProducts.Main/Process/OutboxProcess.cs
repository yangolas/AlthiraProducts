using AlthiraProducts.Adapters.MessageBroker.Publisher.ServicesRegister;
using AlthiraProducts.Adapters.OpenTelemetry;
using AlthiraProducts.Adapters.Outbox;
using AlthiraProducts.Adapters.Outbox.HostInfraestructure;
using AlthiraProducts.Adapters.Outbox.Settings;
using AlthiraProducts.Adapters.Repository.Write;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlthiraProducts.Main.Process;

internal static class OutboxProcess
{
    internal static Task Start(IConfigurationSection configurationSection, AlthiraProductsSettings config)
    {
        IServiceCollection servicesOutboxWorker = new ServiceCollection();
        servicesOutboxWorker.Configure<OutboxSettings>(configurationSection.GetSection("Outbox"));
        servicesOutboxWorker.AddOpenTelemetry(config.OpenTelemetry.OutboxWorkerName, config.OpenTelemetry.ConnectionString);
        servicesOutboxWorker.AddRepositoryContextWrite(config.DatabaseWrite);
        servicesOutboxWorker.AddRepositoryWrite(config.Assembly.AssemblyRepositoryWrite);
        servicesOutboxWorker.AddOutboxPublisherBroker(config.MessageBroker, config.Events);
        servicesOutboxWorker.AddOutboxWorker();

        Task outboxTask = StartOutboxWorker.Main(servicesOutboxWorker);//One worker per db context
        return outboxTask;
    }
}
