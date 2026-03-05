namespace AlthiraProducts.BuildingBlocks.Application.Settings
{
    public class ChannelSettings
    {
        public string EventName { get; set; } = null!;
        public string Exchange { get; set; } = null!;
        public string RoutingKey { get; set; } = null!;
        public string Queue { get; set; } = null!;
        public RetryPolicySettings RetryPolicy { get; set; } = null!;
    }
}