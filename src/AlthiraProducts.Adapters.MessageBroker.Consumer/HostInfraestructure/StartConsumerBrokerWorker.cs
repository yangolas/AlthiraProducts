using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.HostInfraestructure;

public static class StartConsumerBrokerWorker
{
    public static async Task Main(IServiceCollection services)
    {
        using IHost host = Host.CreateDefaultBuilder()
        .ConfigureServices(LocalServices =>
        {
            foreach (var service in services)
            {
                LocalServices.Add(service);
            }
        })
        .Build();

        await host.RunAsync();
    }
}