namespace AlthiraProducts.Main.Settings.Models;

public class RetryPolicySettings
{
    public int MaxRetryAttempts { get; set; }
    public TimeSpan InitialDelay { get; set; }
    public double BackoffMultiplier { get; set; }
}