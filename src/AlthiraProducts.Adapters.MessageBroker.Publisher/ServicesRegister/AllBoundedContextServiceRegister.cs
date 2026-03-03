using AlthiraProducts.Adapters.MessageBroker.Events.Models;
using AlthiraProducts.Adapters.MessageBroker.Publisher.Ports;
using AlthiraProducts.Adapters.MessageBroker.Publisher.Services;
using AlthiraProducts.Main.Settings.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.ServicesRegister;

internal static class AllBoundedContextServiceRegister
{
    internal static void AddAllBoundedContextServices(
        this IServiceCollection services,
        MessageBrokerSettings messageBrokerSettings,
        EventSettings eventSettings)
    {
        services.AddScoped<IPublisherService<Event>, ProductPublisherService>(provider =>
        {
            return new ProductPublisherService(messageBrokerSettings, eventSettings.ProductEventChannels);
        });
    }
}