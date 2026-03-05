using AlthiraProducts.Adapters.Outbox.Settings;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.BuildingBlocks.Application.Ports.Outbox;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AlthiraProducts.Adapters.Outbox.HostInfraestructure;

public class OutboxWorker : BackgroundService
{
    private readonly IOpenTelemetryService _openTelemetryService;
    private readonly ILogger<OutboxWorker> _logger;
    private readonly IOutboxService _outboxService;
    private readonly TimeSpan _intervalToPolling;

    public OutboxWorker(
        IOpenTelemetryService openTelemetryService,
        ILogger<OutboxWorker> logger,
        IOutboxService outboxService,
        IOptions<OutboxSettings> outboxSetting)
    {
        _openTelemetryService = openTelemetryService;
        _logger = logger;
        _outboxService = outboxService;
        _intervalToPolling = outboxSetting.Value.IntervalToPolling;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var activity = _openTelemetryService.StartActivity("AlthiraProducts_OutboxWorker_Initialize");
                await _outboxService.ProcessOutboxAsync();
            }
            catch (Exception ex)
            {
                _openTelemetryService.AddError(ex, "OutboxWorker error to attempted to connect DB");
                _logger.LogError(ex, "OutboxWorker error to attempted to connect DB");
            }

            await Task.Delay(_intervalToPolling, stoppingToken);
        }
    }
}