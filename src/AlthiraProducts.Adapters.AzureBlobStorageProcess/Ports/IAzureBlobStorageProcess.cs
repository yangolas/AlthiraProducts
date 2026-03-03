namespace AlthiraProducts.Adapters.AzureBlobStorageProcess.Ports;

public interface IAzureBlobStorageProcess
{
    public TimeSpan IntervalToPolling { get;}
    Task Process();
}
