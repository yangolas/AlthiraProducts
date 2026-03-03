namespace AlthiraProducts.Main.Settings.Models;

public class BlobContainerSettings
{
    public string Name { get; set; } = null!;
    public int WorkerBatchSize { get; set; }
    public TimeSpan IntervalToPolling { get; set; }
    public RetryPolicySettings RetryPolicy { get; set; } = null!;
}