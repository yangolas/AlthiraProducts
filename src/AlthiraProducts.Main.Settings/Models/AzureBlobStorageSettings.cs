namespace AlthiraProducts.Main.Settings.Models;

public class AzureBlobStorageSettings
{
    public string ConnectionString { get; set; } = null!;

    public BlobContainerSettings ProductImageBlobContainer { get; set; } = null!;
}