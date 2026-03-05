using AlthiraProducts.BuildingBlocks.Application.Models.Blobs;
using AlthiraProducts.BuildingBlocks.Application.Ports.AzureBlobStorage;

namespace AlthiraProducts.Adapters.AzureBlobStorageProcess.Services;

public class AzureBlobStorageProcess(IAzureBlobStorageService _azureBlobStorageService)
{
    public async Task MoveBlobFromTempToContainer(string blobName, string contentType)
    {
        Stream tempImage = await _azureBlobStorageService.DownloadAsync(
            blobName,
            isTemp: true
        );
        await _azureBlobStorageService.UploadBlobAsync(
            new BlobModel() 
            { 
                Name = blobName,
                Content = tempImage,
                ContentType = contentType,
            },
            isTemp: false
        );
        await _azureBlobStorageService.DeleteAsync(
            blobName,
            isTemp: true
        );
    }
}