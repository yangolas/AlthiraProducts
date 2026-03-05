namespace AlthiraProducts.BuildingBlocks.Application.Models.Blobs;

public class BlobModel
{
    public string Name { get; set; } = null!;
    public Stream Content{ get; set; } = null!;
    public string ContentType{ get; set; } = null!;
}