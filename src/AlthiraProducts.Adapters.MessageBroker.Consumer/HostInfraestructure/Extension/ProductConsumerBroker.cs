using AlthiraProducts.Adapters.MessageBroker.Consumer.Ports.Extensions;
using AlthiraProducts.Adapters.MessageBroker.Events.Models;
using AlthiraProducts.Adapters.OpenTelemetry.Ports;
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