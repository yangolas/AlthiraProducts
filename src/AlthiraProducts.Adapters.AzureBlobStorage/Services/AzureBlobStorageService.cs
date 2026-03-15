using AlthiraProducts.BuildingBlocks.Application.Models.Blobs;
using AlthiraProducts.BuildingBlocks.Application.Ports.AzureBlobStorage;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.AzureBlobStorage.Services;

public class AzureBlobStorageService : IAzureBlobStorageService
{
    private readonly ILogger<AzureBlobStorageService> _logger;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerTemp;
    private readonly BlobContainerClient _container;
    private readonly IngressLocalKubernetesSettings? _ingressLocalKubernetes;
    public AzureBlobStorageService(
        ILogger<AzureBlobStorageService> logger,
        string connectionString,
        string containerName,
        IngressLocalKubernetesSettings? ingressLocalKubernetesSettings)
    {
        _logger = logger;
        _blobServiceClient = new BlobServiceClient(connectionString);
        _containerTemp = _blobServiceClient.GetBlobContainerClient($"{containerName}-temp");
        _container = _blobServiceClient.GetBlobContainerClient(containerName);
        _ingressLocalKubernetes = ingressLocalKubernetesSettings;
    }

    private BlobContainerClient GetContainer(bool isTemp) => isTemp ? _containerTemp : _container;

    public async Task UploadBlobAsync(BlobModel blobModel, bool isTemp = false)
    {
        try
        {
            BlobContainerClient container = GetContainer(isTemp);
            Console.WriteLine($"[DEBUG] Ejecutando CreateIfNotExistsAsync para el contenedor: {container.Name}...");
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
            await container.SetAccessPolicyAsync(PublicAccessType.Blob);
            Console.WriteLine($"[DEBUG] Contenedor verificado/creado con éxito.");

            BlobClient blob = container.GetBlobClient(blobModel.Name);

            blobModel.Content.Position = 0;

            BlobUploadOptions uploadOptions = new()
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = blobModel.ContentType
                }
            };
            Console.WriteLine($"[DEBUG] Subida finalizada. URI: {blob.Uri}");
            await blob.UploadAsync(blobModel.Content, uploadOptions);
        }
        catch (Exception ex) {
            // LOG 3: El error detallado
            Console.WriteLine($"[ERROR FATAL] Error en UploadAsync: {ex.Message}");
            Console.WriteLine($"[STACKTRACE] {ex.StackTrace}");
            throw;
        }
    }

    public async Task UploadBlobsAsync(IEnumerable<BlobModel> blobsModel, bool isTemp = false)
    {
        foreach (BlobModel blobModel  in blobsModel)
        {
            await UploadBlobAsync(blobModel, isTemp);
        }
    }

    public async Task<Stream> DownloadAsync(string fileName, bool isTemp = false)
    {
        BlobContainerClient container = GetContainer(isTemp);

        BlobClient blob = container.GetBlobClient(fileName);

        BlobDownloadInfo download = await blob.DownloadAsync();

        MemoryStream stream = new ();
        await download.Content.CopyToAsync(stream);

        stream.Position = 0;

        return stream;
    }

    public async Task<Dictionary<string, Stream>> DownloadAsync(IEnumerable<string> blobNames, bool isTemp = false)
    {
        Dictionary<string, Stream> dictionaryFileNameStream = [];

        foreach (string name in blobNames)
        {
            Stream stream = await DownloadAsync(name, isTemp);
            dictionaryFileNameStream.Add(name, stream);
        }

        return dictionaryFileNameStream;
    }

    public async Task DeleteAsync( string fileName, bool isTemp = false)
    {
        BlobContainerClient container = GetContainer(isTemp);
        BlobClient blob = container.GetBlobClient(fileName);
        await blob.DeleteIfExistsAsync();
    }

    public async Task DeleteAsync(IEnumerable<string> filesName, bool isTemp = false)
    {
        foreach (string fileName in filesName)
        {
            await DeleteAsync(fileName, isTemp);
        }
    }

    public string GetReadOnlySasUri(string blobName, bool isTemp = false)
    {
        var containerClient = GetContainer(isTemp);
        var blobClient = containerClient.GetBlobClient(blobName);

        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = containerClient.Name,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1) 
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

        return _ingressLocalKubernetes == null
            ? sasUri.ToString()
            : GetSasUriForLocalKubernetes(sasUri, containerClient.AccountName);
    }

    private string GetSasUriForLocalKubernetes(Uri sasUri, string accountName)
    {
        _ = _ingressLocalKubernetes
            ?? throw new NullReferenceException("Settings for local kubernetes ingress not configured");

        var builder = new BlobUriBuilder(sasUri)
        {
            Host = _ingressLocalKubernetes.Host,
            Port = _ingressLocalKubernetes.Port,
            AccountName = accountName
        };

        return builder.ToUri().ToString();
    }
}