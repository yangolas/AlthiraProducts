using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using AlthiraProducts.Products.Application.Ports.MessageBrokerPublisher;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.Services.Extenisons;

public class ProductPublisherService : PublisherService<Event>,IProductPublisherService
{
    public ProductPublisherService(
        ILogger<ProductPublisherService> logger,
        IOpenTelemetryService openTelemetryService,
        MessageBrokerSettings messageBrokerSettings,
        ChannelSettings[] channelSettings)
        : base(
            logger,
            openTelemetryService,
            messageBrokerSettings,
            channelSettings)
    {
    }
}