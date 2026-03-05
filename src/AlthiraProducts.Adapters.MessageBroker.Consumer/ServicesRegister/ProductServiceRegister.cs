using AlthiraProducts.Adapters.MessageBroker.Consumer.HostInfraestructure.Extension;
using AlthiraProducts.Adapters.MessageBroker.Consumer.Services.Extensions;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using AlthiraProducts.Products.Application.Ports.MessageBrokerConsumer;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
                ILogger<ProductConsumerService> logger = provider.GetRequiredService<ILogger<ProductConsumerService>>();
                IOpenTelemetryService openTelemetryService = provider.GetRequiredService<IOpenTelemetryService>();
                return new ProductConsumerService(logger, openTelemetryService, messageBrokerSettings, productEventChannel, mediator);
            });
        }

        services.AddHostedService<ProductConsumerBroker>();
    }
}