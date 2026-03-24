using AlthiraProducts.Adapters.MessageBroker.Consumer.HostInfraestructure;
using AlthiraProducts.Adapters.MessageBroker.Consumer.ServicesRegister;
using AlthiraProducts.Adapters.OpenTelemetry;
using AlthiraProducts.Adapters.Repository.Read.Services;
using AlthiraProducts.BoundedContext.Products.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlthiraProducts.Main.Process;

internal static class ConsumerProcess
{
    internal static Task Start(IConfigurationSection configurationSection, AlthiraProductsSettings config)
    {
        IServiceCollection servicesConsumerBrokerMessage = new ServiceCollection();
        servicesConsumerBrokerMessage.Configure<AlthiraProductsSettings>(configurationSection);
        servicesConsumerBrokerMessage.AddOpenTelemetry(config.OpenTelemetry.ConsumerBrokerName, config.OpenTelemetry.ConnectionString);
        servicesConsumerBrokerMessage.AddApplicationProduct(config.Assembly.AssemblyApplicationProduct);
        servicesConsumerBrokerMessage.AddHostedConsumerBroker(config.MessageBroker, config.Events);
        servicesConsumerBrokerMessage.AddRepositoryContextRead(config.DatabaseRead);
        servicesConsumerBrokerMessage.AddRepositoryRead(config.Assembly.AssemblyRepositoryRead);

        Task consumerBrokerTask = StartConsumerBrokerWorker.Main(servicesConsumerBrokerMessage);// One Worker per Bounded Context, N Consumers per Bounded Context 
        return consumerBrokerTask;
    }
}
