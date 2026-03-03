using AlthiraProducts.Main.Settings.Models;

public class AlthiraProductsSettings
{
    public OpenTelemetrySettings OpenTelemetry { get; set; } = null!;
    public DatabaseReadSettings DatabaseRead { get; set; } = null!;
    public DatabaseWriteSettings DatabaseWrite { get; set; } = null!;
    public MessageBrokerSettings MessageBroker { get; set; } = null!;
    public OutboxSettings Outbox { get; set; } = null!;
    public EventSettings Events { get; set; } = null!;
    public LogSettings Logs { get; set; } = null!;
    public AuthenticationSettings Authentication { get; set; } = null!;
    public AssemblySettings Assembly { get; set; } = null!;
    public AzureBlobStorageSettings AzureBlobStorage { get; set; } = null!;
}