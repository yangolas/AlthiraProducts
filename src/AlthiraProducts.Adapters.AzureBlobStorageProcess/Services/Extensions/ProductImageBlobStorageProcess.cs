using AlthiraProducts.Adapters.AzureBlobStorage.Ports.Extensions;
using AlthiraProducts.Adapters.AzureBlobStorageProcess.Diagnostic;
using AlthiraProducts.Adapters.AzureBlobStorageProcess.Ports;
using AlthiraProducts.Adapters.OpenTelemetry.Ports;
using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;
using AlthiraProducts.Adapters.Repository.Write.Ports;
using AlthiraProducts.Adapters.Repository.Write.Ports.Products;
using AlthiraProducts.Main.Settings.Models;
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
        IOptions<AlthiraProductsSettings> appsettings,
        IImageRepositoryWrite imageRepositoryWrite,
        IProductImageBlobStorageService productImageBlobStorageService,
        IUnitOfWork unitOfWork)
        :base(productImageBlobStorageService)
    {
        _logger = logger;
        _openTelemetryService = openTelemetryService;
        BlobContainerSettings blobcontainersettings = appsettings.Value.AzureBlobStorage.ProductImageBlobContainer;
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