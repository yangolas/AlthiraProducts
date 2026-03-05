using AlthiraProducts.BuildingBlocks.Application.Settings;

namespace AlthiraProducts.Adapters.Outbox.Settings;

public class OutboxSettings
{
    public int BatchSize { get; set; }
    public TimeSpan IntervalToPolling { get; set; }
    public RetryPolicySettings RetryPolicy { get; set; } = null!;
}