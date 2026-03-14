using AlthiraProducts.BuildingBlocks.Application.Models.Blobs;
using AlthiraProducts.BuildingBlocks.Application.Ports.AzureBlobStorage;
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
    public AzureBlobStorageService(
        ILogger<AzureBlobStorageService> logger,
        string connectionString,
        string containerName)
    {
        _logger = logger;

        // FORZAMOS LA VERSIÓN MÁS ANTIGUA POSIBLE QUE SOPORTE .NET 10
        // A veces las versiones nuevas envían cabeceras de "Token" que Azurite 3.35 no entiende
        var options = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2019_02_02);

        // USAMOS LA CADENA DIRECTA PARA QUE EL SDK NO "ADIVINE" NADA
        // Importante: No debe llevar el nombre de la cuenta al final de los endpoints
        string k8sConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://azurite-srv.althira.svc.cluster.local:10000/devstoreaccount1;";

        _blobServiceClient = new BlobServiceClient(k8sConnectionString, options);
        _containerTemp = _blobServiceClient.GetBlobContainerClient($"{containerName}-temp");
        _container = _blobServiceClient.GetBlobContainerClient(containerName);
    }

    private BlobContainerClient GetContainer(bool isTemp) => isTemp ? _containerTemp : _container;

    public async Task UploadBlobAsync(BlobModel blobModel, bool isTemp = false)
    {
        try
        {
            BlobContainerClient container = GetContainer(isTemp);
            Console.WriteLine($"[DEBUG] Ejecutando CreateIfNotExistsAsync para el contenedor: {container.Name}...");
            await container.CreateIfNotExistsAsync();
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

        return blobClient.GenerateSasUri(sasBuilder).ToString();
    }
}