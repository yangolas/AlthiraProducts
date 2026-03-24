using AlthiraProducts.Adapters.MessageBroker.Consumer.Services.Extensions;
using AlthiraProducts.Adapters.MessageBroker.Publisher.Services.Extenisons;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using AlthiraProducts.Products.Application.Ports.MessageBrokerPublisher;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            ILogger<ProductPublisherService> logger = provider.GetRequiredService<ILogger<ProductPublisherService>>();
            IOpenTelemetryService openTelemetryService = provider.GetRequiredService<IOpenTelemetryService>();
            return new ProductPublisherService(logger, openTelemetryService, messageBrokerSettings, productEventChannels);
        });
    }
}
