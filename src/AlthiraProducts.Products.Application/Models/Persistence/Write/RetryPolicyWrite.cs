namespace AlthiraProducts.Products.Application.Models.Persistence.Write;


public abstract class RetryPolicyWrite<TStatus>
where TStatus : Enum
{
    public int RetryCount { get; protected set; }
    public TStatus Status { get; protected set; } = default!;
    public DateTime? NextRetryAt { get; protected set; }
    public DateTime? ProcessedAt { get; protected set; }
    public DateTime InsertedAt { get; protected set; }
    public string? Error { get; protected set; }
    public string? TraceContext { get; protected set; }

    protected void InitializeProcessable(TStatus pendingStatus, string? traceContxt = null)
    {
        InsertedAt = DateTime.UtcNow;
        Status = pendingStatus;
        RetryCount = 0;
        TraceContext = traceContxt;
    }

    protected void RegisterRetry(
        string error,
        TimeSpan initialDelay,
        double backoffFactor,
        TStatus pendingStatus)
    {
        RetryCount++;

        // La fórmula matemática centralizada
        var delaySeconds = initialDelay.TotalSeconds * Math.Pow(backoffFactor, RetryCount - 1);
        NextRetryAt = DateTime.UtcNow.AddSeconds(delaySeconds);

        Status = pendingStatus;
        ProcessedAt = null;
        Error = error;
    }

    protected void MarkAsProcessed(TStatus processedStatus)
    {
        Status = processedStatus;
        ProcessedAt = DateTime.UtcNow;
        Error = null;
        NextRetryAt = null;
    }

    protected void MarkAsFailed(string error, TStatus failedStatus)
    {
        Status = failedStatus;
        ProcessedAt = null;
        Error = error;
        NextRetryAt = null;
    }
}