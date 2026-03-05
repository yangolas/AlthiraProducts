using AlthiraProducts.BuildingBlocks.Application.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.ServicesRegister;

public static class ServiceRegister
{
    public static void AddPublisherBroker(
        this IServiceCollection services, 
        MessageBrokerSettings messageBrokerSettings, 
        EventSettings eventSettings) 
    {

        ProductServiceRegister.AddProduct(
            services, 
            messageBrokerSettings,
            eventSettings.ProductEventChannels);
    }

    public static void AddOutboxPublisherBroker(
        this IServiceCollection services,
        MessageBrokerSettings messageBrokerSettings,
        EventSettings eventSettings)
    {
        services.AddAllBoundedContextServices(
         messageBrokerSettings,
         eventSettings);
    }
}