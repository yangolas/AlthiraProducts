using AlthiraProducts.Adapters.MessageBroker.Consumer.Ports;
using AlthiraProducts.Adapters.MessageBroker.Events.Models;
using AlthiraProducts.Adapters.OpenTelemetry.Ports;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.MessageBroker.Consumer.HostInfraestructure;

public abstract class ConsumerBroker<TEvent> : BackgroundService where TEvent : Event
{
    private readonly ILogger _logger;
    private readonly IOpenTelemetryService _openTelemetryService;
    private readonly IEnumerable<IConsumerService<TEvent>> _consumersService;

    public ConsumerBroker(
        ILogger logger,
        IOpenTelemetryService openTelemetryService,
        IEnumerable<IConsumerService<TEvent>> consumersService)
    {
        _logger = logger;
        _openTelemetryService = openTelemetryService;
        _consumersService = consumersService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (IConsumerService<TEvent> consumerService in _consumersService) 
        { 
            _openTelemetryService.StartInternalActivity($"AlthiraProducts_Consumer_{consumerService.EventName}_Initialize");
            _logger.LogInformation("Starting consumer service: {ConsumerServiceName}", consumerService.EventName);
            await consumerService.ConsumeMediatRAsync(stoppingToken);
        }
    }
}