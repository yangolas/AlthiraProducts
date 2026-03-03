using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AlthiraProducts.Adapters.Outbox.HostInfraestructure;

public class StartOutboxWorker
{
    public static async Task Main(IServiceCollection services)
    {
        using IHost host = Host.CreateDefaultBuilder()
        .ConfigureServices(localServices =>
        {
            foreach (var service in services)
            {
                localServices.Add(service);
            }
        })
        .Build();

        await host.RunAsync();
    }
}