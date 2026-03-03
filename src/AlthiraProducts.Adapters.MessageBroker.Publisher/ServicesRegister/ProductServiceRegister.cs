using AlthiraProducts.Adapters.MessageBroker.Publisher.Ports.Extensions;
using AlthiraProducts.Adapters.MessageBroker.Publisher.Services;
using AlthiraProducts.Main.Settings.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.ServicesRegister;

internal static class ProductServiceRegister
{

    internal static void AddProduct(
        IServiceCollection services,
        MessageBrokerSettings messageBrokerSettings,
        ChannelSettings[] productEventChannels)
    {
        services.AddScoped<IProductPublisherService, ProductPublisherService>(provider =>
        {
            return new ProductPublisherService(messageBrokerSettings, productEventChannels);
        });
    }
}
