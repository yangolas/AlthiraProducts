namespace AlthiraProducts.Adapters.AzureBlobStorage.Models;

public class BlobModel
{
    public string Name { get; set; } = null!;
    public Stream Content{ get; set; } = null!;
    public string ContentType{ get; set; } = null!;
}