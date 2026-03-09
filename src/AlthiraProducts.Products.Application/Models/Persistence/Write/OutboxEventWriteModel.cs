using AlthiraProducts.Products.Application.Ports.RepositoryWrite.Enums;

namespace AlthiraProducts.Products.Application.Models.Persistence.Write;

public class OutboxEventWriteModel : RetryPolicyWrite<OutboxStatus>
{
    public Guid Id { get; private set; }
    public string EventName { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public int Version { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private OutboxEventWriteModel() { }

    public static OutboxEventWriteModel Create(
        string eventName,
        int version,
        string payload,
        DateTime createdAt,
        string? traceContext = null)
    {
        OutboxEventWriteModel outboxEventWriteModel = new ()
        {
            Id = Guid.NewGuid(),
            EventName = eventName,
            Payload = payload,
            Version = version,
            CreatedAt = createdAt
        };

        outboxEventWriteModel.InitializeProcessable(OutboxStatus.Pending, traceContext);
        return outboxEventWriteModel;
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
            OutboxStatus.Pending);
    }

    public void MarkAsProcessed()
    {
        MarkAsProcessed(OutboxStatus.Processed);
    }

    public void MarkAsFailed(string error)
    {
        MarkAsFailed(error, OutboxStatus.Failed);
    }
}
