namespace AlthiraProducts.BuildingBlocks.Application.Settings;

public class RetryPolicySettings
{
    public int MaxRetryAttempts { get; set; }
    public TimeSpan InitialDelay { get; set; }
    public double BackoffMultiplier { get; set; }
}