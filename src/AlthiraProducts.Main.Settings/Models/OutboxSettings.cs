namespace AlthiraProducts.Main.Settings.Models;

public class OutboxSettings
{
    public int BatchSize { get; set; }
    public TimeSpan IntervalToPolling { get; set; }
    public RetryPolicySettings RetryPolicy { get; set; } = null!;
}