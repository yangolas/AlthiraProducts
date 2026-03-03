using AlthiraProducts.Main.Settings.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.ServicesRegister;

public static class ServiceRegister
{
    public static void AddConsumerBroker(this IServiceCollection services, MessageBrokerSettings messageBrokerSettings, EventSettings eventSettings)
    {
        //only if it is narrow necessary listen and wait to the queue it is a bad practise
    }

    public static void AddHostedConsumerBroker(this IServiceCollection services, MessageBrokerSettings messageBrokerSettings, EventSettings eventSettings)
    {
        ProductServiceRegister.AddProductsHosted(services, messageBrokerSettings, eventSettings.ProductEventChannels);
    }
}
