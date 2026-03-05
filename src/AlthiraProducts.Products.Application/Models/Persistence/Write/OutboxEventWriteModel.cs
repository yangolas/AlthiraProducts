using AlthiraProducts.Products.Application.Ports.RepositoryWrite.Enums;

namespace AlthiraProducts.Products.Application.Models.Persistence.Write;

public class OutboxEventWriteModel : RetryPolicyWrite<OutboxStatus>
{
    public Guid Id { get; private set; }
    public string EventName { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public int Version { get; private set; }


    private OutboxEventWriteModel() { }

    public static OutboxEventWriteModel Create(
        string eventName,
        int version,
        string payload)
    {
        OutboxEventWriteModel outboxEventWriteModel = new ()
        {
            Id = Guid.NewGuid(),
            EventName = eventName,
            Payload = payload,
            Version = version
        };

        outboxEventWriteModel.InitializeProcessable(OutboxStatus.Pending);
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
