using AlthiraProducts.Adapters.AzureBlobStorageProcess.Diagnostic;
using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Write;
using AlthiraProducts.BoundedContext.Products.Application.Ports.AzureBlobSotageProcess;
using AlthiraProducts.BoundedContext.Products.Application.Ports.AzureBlobStorage;
using AlthiraProducts.BoundedContext.Products.Application.Ports.RepositoryWrite;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.BuildingBlocks.Application.Ports.RepositoryWrite;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AlthiraProducts.Adapters.AzureBlobStorageProcess.Services.Extensions;

public class ProductImageBlobStorageProcess : AzureBlobStorageProcess, IProductImageBlobStorageProcess
{
    private readonly ILogger<ProductImageBlobStorageProcess> _logger;
    private readonly IOpenTelemetryService _openTelemetryService;
    private readonly IImageRepositoryWrite _imageRepositoryWrite;
    private readonly IUnitOfWork _unitOfWork;
    private readonly int _batchSize;
    private readonly int _maxRetryAttempts;
    private readonly TimeSpan _initialDelay;
    private readonly double _backoffFactor;
    public TimeSpan IntervalToPolling { get; init; }

    public ProductImageBlobStorageProcess(
        ILogger<ProductImageBlobStorageProcess> logger,
        IOpenTelemetryService openTelemetryService,
        IOptions<AzureBlobStorageSettings> azureBlobStorageSettings,
        IImageRepositoryWrite imageRepositoryWrite,
        IProductImageBlobStorageService productImageBlobStorageService,
        IUnitOfWork unitOfWork)
        :base(productImageBlobStorageService)
    {
        _logger = logger;
        _openTelemetryService = openTelemetryService;
        BlobContainerSettings blobcontainersettings = azureBlobStorageSettings.Value.ProductImageBlobContainer;
        IntervalToPolling = blobcontainersettings.IntervalToPolling;
        _batchSize = blobcontainersettings.WorkerBatchSize;
        RetryPolicySettings retryPolicy = blobcontainersettings.RetryPolicy;
        _maxRetryAttempts = retryPolicy.MaxRetryAttempts;
        _initialDelay =retryPolicy.InitialDelay;
        _backoffFactor = retryPolicy.BackoffMultiplier;
        _imageRepositoryWrite = imageRepositoryWrite;
        _unitOfWork = unitOfWork;
    }

    private void HandleRetryPolicy(
        ProductImageWriteModel image,
        Exception ex)
    {
        image.RegisterRetry(
            ex.Message,
            _initialDelay,
            _backoffFactor);

        if (image.RetryCount >= _maxRetryAttempts)
        {
            image.MarkAsFailed(ex.Message);
        }
    }

    public async Task Process()
    {
        IEnumerable<ProductImageWriteModel> images = await _imageRepositoryWrite.GetProductsWithPendingImagesAsync(_batchSize);
        foreach (ProductImageWriteModel image in images)
        {
            try
            {
                _openTelemetryService.AddProductBlobStorageMetada(image);
                await MoveBlobFromTempToContainer(image.Name.ToString(), image.ContentType);

                image.MarkAsProcessed();
            }
            catch (Exception ex)
            {
                HandleRetryPolicy(image, ex);
                _openTelemetryService.AddError(ex, "Failed to move image container temp to original container");
                _logger.LogError(ex, "Failed to move image container temp to original container {ImageName}", image.Name);
            }
        }
        await _unitOfWork.SaveChangesAsync();
    }
}