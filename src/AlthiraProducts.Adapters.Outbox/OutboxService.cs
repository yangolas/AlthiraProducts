using AlthiraProducts.Adapters.MessageBroker.Events.Models;
using AlthiraProducts.Adapters.MessageBroker.Publisher.Ports;
using AlthiraProducts.Adapters.OpenTelemetry.Ports;
using AlthiraProducts.Adapters.Outbox.Diagnostic.Telemetry;
using AlthiraProducts.Adapters.Outbox.Mappers;
using AlthiraProducts.Adapters.Outbox.Ports;
using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;
using AlthiraProducts.Adapters.Repository.Write.Ports;
using AlthiraProducts.Adapters.Repository.Write.Ports.Products;
using AlthiraProducts.Main.Settings.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace AlthiraProducts.Adapters.Outbox;

public class OutboxService : IOutboxService
{
    private readonly ILogger<OutboxService> _logger;
    private readonly IOpenTelemetryService _openTelemetryService;
    private readonly OutboxSettings _outboxSettings;
    private readonly IOutboxEventRepositoryWrite _outboxRepository;
    private readonly IEnumerable<IPublisherService<Event>> _publishersSevice;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ConcurrentDictionary<Type, object> _publisherCache = new();
    private readonly Dictionary<string, IPublisherService<Event>> dictionaryEventNamePublishService = [];

    public OutboxService(
        IOptions<AlthiraProductsSettings> appSettings,
        ILogger<OutboxService> logger,
        IOpenTelemetryService openTelemeryService,
        IEnumerable<IPublisherService<Event>> publishersService,
        IOutboxEventRepositoryWrite outboxRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _openTelemetryService = openTelemeryService;
        _outboxSettings = appSettings.Value.Outbox;
        _publishersSevice = publishersService;
        _outboxRepository = outboxRepository;
        _unitOfWork = unitOfWork;
        DictionaryEventNamePublisher(_publishersSevice);
    }

    private void DictionaryEventNamePublisher(IEnumerable<IPublisherService<Event>> publishers)
    {
        foreach (var publisher in publishers) 
        { 
            foreach (var eventName in publisher.GetEventsName())
            {
                if (!dictionaryEventNamePublishService.TryAdd(eventName, publisher))
                {
                    throw new InvalidOperationException(
                        $"The event '{eventName}' is already registered.");
                }
            }
        }
    }

    public void HandleRetryPolicy(
        OutboxEventWriteModel outboxEvent,
        Exception ex)
    {
        outboxEvent.RegisterRetry(
            ex.Message,
            _outboxSettings.RetryPolicy.InitialDelay,
            _outboxSettings.RetryPolicy.BackoffMultiplier);

        if (outboxEvent.RetryCount >= _outboxSettings.RetryPolicy.MaxRetryAttempts)
        {
            outboxEvent.MarkAsFailed(ex.Message);
            return;
        }
    }

    public async Task ProcessOutboxAsync()
    {
        IEnumerable<OutboxEventWriteModel> outboxEvents =
                await _outboxRepository.GetPendingEventsAsync(_outboxSettings.BatchSize);

        foreach (OutboxEventWriteModel outboxEvent in outboxEvents)
        {
            using var activity = _openTelemetryService.StartInternalActivity("ProcessOutboxEvent");
            _openTelemetryService.AddOutboxMetadata(outboxEvent);
            try
            {
                Event @event = outboxEvent.MapToEvent();

                string eventName = @event.EventName;
                var publisherService = dictionaryEventNamePublishService[eventName];

                await publisherService.PublishAsync((dynamic)@event);
                outboxEvent.MarkAsProcessed();
            }
            catch (Exception ex)
            {
                _openTelemetryService.AddError(ex, "Failed to publish outbox event");
                _logger.LogError(ex, "Error publishing event {EventId}, network connectivity issue", outboxEvent.Id);
                HandleRetryPolicy(outboxEvent, ex);
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }
}