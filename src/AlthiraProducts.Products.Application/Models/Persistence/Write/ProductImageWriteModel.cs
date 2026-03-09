using AlthiraProducts.Products.Application.Ports.RepositoryWrite.Enums;

namespace AlthiraProducts.Products.Application.Models.Persistence.Write;

public class ProductImageWriteModel: RetryPolicyWrite<ImageStatus>
{
    public Guid Name { get; private set; }
    public string ContentType { get; private set; } = null!;
    public int Order { get; private set; }
    public ProductWriteModel Product { get; private set; } = null!;
    public Guid ProductId { get; private set; }
    private ProductImageWriteModel() {}

    public static ProductImageWriteModel Create(
        Guid name,
        string contentType,
        int order,
        Guid productId,
        string? traceContext = null)
    {
        ProductImageWriteModel productImageWriteModel = new() 
        {
            Name = name,
            ContentType = contentType,
            Order = order,
            ProductId = productId
        };

        productImageWriteModel.InitializeProcessable(
            ImageStatus.Pending,
            traceContext);

        return productImageWriteModel;
    }

    public void RegisterRetry(
        string error,
        TimeSpan initialDelay,
        double backoffFactor)
    {
        RegisterRetry(
            error,
            initialDelay,
            backoffFactor,
            ImageStatus.Pending);
    }

    public void MarkAsProcessed()
    {
        MarkAsProcessed(ImageStatus.Processed);
    }

    public void MarkAsFailed(string error)
    {
        MarkAsFailed(error, ImageStatus.Failed);
    }
}