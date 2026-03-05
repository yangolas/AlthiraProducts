namespace AlthiraProducts.BuildingBlocks.Application.Ports.AzureBlobSotageProcess;

public interface IAzureBlobStorageProcess
{
    public TimeSpan IntervalToPolling { get;}
    Task Process();
}
