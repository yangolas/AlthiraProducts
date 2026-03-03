public class OpenTelemetrySettings
{
    public string ConnectionString { get; set; } = null!;
    public string WebApiName { get; set; } = null!;
    public string ConsumerBrokerName { get; set; } = null!;
    public string OutboxWorkerName { get; set; } = null!;
    public string AzureBlobStorageWorkerName { get; set; } = null!;
}