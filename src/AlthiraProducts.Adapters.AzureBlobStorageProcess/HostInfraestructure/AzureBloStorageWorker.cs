using AlthiraProducts.Adapters.AzureBlobStorageProcess.Ports;
using AlthiraProducts.Adapters.OpenTelemetry.Ports;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.AzureBlobStorageProcess.HostInfraestructure;

public class AzureBloStorageWorker : BackgroundService
{
    private readonly ILogger<AzureBloStorageWorker> _logger;
    private readonly IOpenTelemetryService _openTelemetryService;
    private readonly IAzureBlobStorageProcess _azureBlobStorageProcess;
    private readonly TimeSpan _intervalToPolling;

    public AzureBloStorageWorker(
        ILogger<AzureBloStorageWorker> logger,
        IOpenTelemetryService openTelemetryService,
        IAzureBlobStorageProcess azureBlobStorageProcess)
    {
        _logger = logger;
        _openTelemetryService = openTelemetryService;
        _azureBlobStorageProcess = azureBlobStorageProcess;
        _intervalToPolling = azureBlobStorageProcess.IntervalToPolling;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _openTelemetryService.StartActivity($"AlthiraProducts_AzureBlobStorageWorker_{_azureBlobStorageProcess.GetType().Name}_Initialize");
                await _azureBlobStorageProcess.Process();
            }
            catch (Exception ex)
            {
                _openTelemetryService.AddError(ex, "Azure blob storage worker error to attempted to connect DB");
                _logger.LogError(ex, $"Error processing azure blob storage: {_azureBlobStorageProcess.GetType().Name}");
            }

            await Task.Delay(_intervalToPolling, stoppingToken);
        }
    }

}