namespace AlthiraProducts.BuildingBlocks.Application.Settings;

public class AzureBlobStorageSettings
{
    public string ConnectionString { get; set; } = null!;
    public IngressLocalKubernetesSettings? IngressLocalKubernetes { get; set; }
    public BlobContainerSettings ProductImageBlobContainer { get; set; } = null!;
}