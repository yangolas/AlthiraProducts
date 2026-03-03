using AlthiraProducts.Adapters.MessageBroker.Consumer.HostInfraestructure.Extension;
using AlthiraProducts.Adapters.MessageBroker.Consumer.Ports.Extensions;
using AlthiraProducts.Adapters.MessageBroker.Consumer.Services.Extensions;
using AlthiraProducts.Main.Settings.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.ServicesRegister;

internal static class ProductServiceRegister
{
    internal static void AddProducts(this IServiceCollection services, MessageBrokerSettings messageBrokerSettings, ChannelSettings[] productEventChannels)
    {

    }

    internal static void AddProductsHosted(this IServiceCollection services, MessageBrokerSettings messageBrokerSettings, ChannelSettings[] productEventChannels)
    {
        foreach (ChannelSettings productEventChannel in productEventChannels)
        {
            services.AddSingleton<IProductConsumerService, ProductConsumerService>(provider =>
            {
                IMediator mediator = provider.GetRequiredService<IMediator>();
                return new ProductConsumerService(messageBrokerSettings, productEventChannel, mediator);
            });
        }

        services.AddHostedService<ProductConsumerBroker>();
    }
}