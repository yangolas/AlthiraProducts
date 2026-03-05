using AlthiraProducts.BuildingBlocks.Application.Models.Blobs;

namespace AlthiraProducts.BuildingBlocks.Application.Ports.AzureBlobStorage;

public interface IAzureBlobStorageService
{
    Task UploadBlobAsync(BlobModel blobModel, bool isTemp=false);

    Task UploadBlobsAsync(IEnumerable<BlobModel> blobModels, bool isTemp = false);

    Task<Stream> DownloadAsync(string fileName, bool isTemp = false);

    Task<Dictionary<string, Stream>> DownloadAsync(IEnumerable<string> blobNames, bool isTemp = false);

    Task DeleteAsync(string fileName, bool isTemp = false);

    Task DeleteAsync(IEnumerable<string> filesName, bool isTemp = false);

    string GetReadOnlySasUri(string blobName, bool isTemp = false);
}