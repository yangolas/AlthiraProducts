using AlthiraProducts.BoundedContext.Products.Application.Ports.MessageBrokerConsumer;
using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.HostInfraestructure.Extension;

public class ProductConsumerBroker : ConsumerBroker<Event>
{
    public ProductConsumerBroker(
        ILogger<ProductConsumerBroker> logger,
        IOpenTelemetryService openTelemetryService,
        IEnumerable<IProductConsumerService> consumersService)
        : base(logger, openTelemetryService, consumersService)
    {
    }
}