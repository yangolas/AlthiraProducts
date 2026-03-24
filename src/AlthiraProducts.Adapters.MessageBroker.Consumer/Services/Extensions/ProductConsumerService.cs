using AlthiraProducts.BoundedContext.Products.Application.Ports.MessageBrokerConsumer;
using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.Services.Extensions;

public class ProductConsumerService : ConsumerService<Event>, IProductConsumerService
{
    public ProductConsumerService(
        ILogger<ProductConsumerService> logger,
        IOpenTelemetryService openTelemetryService,
        MessageBrokerSettings messageBrokerSettings,
        ChannelSettings channelSettings,
        IMediator mediator)
        : base( logger, openTelemetryService, messageBrokerSettings, channelSettings, mediator)
    {
    }
}
