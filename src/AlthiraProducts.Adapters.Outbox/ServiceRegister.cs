using AlthiraProducts.Adapters.Outbox.HostInfraestructure;
using AlthiraProducts.BuildingBlocks.Application.Ports.Outbox;
using Microsoft.Extensions.DependencyInjection;

namespace AlthiraProducts.Adapters.Outbox;

public static class ServiceRegister
{
    public static void AddOutboxWorker(this IServiceCollection services)
    {
        services.AddSingleton<IOutboxService, OutboxService>();

        services.AddHostedService<OutboxWorker>();
    }
}