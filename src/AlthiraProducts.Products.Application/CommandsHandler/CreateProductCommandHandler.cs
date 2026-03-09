using AlthiraProducts.BuildingBlocks.Application.Models.Blobs;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.BuildingBlocks.Application.Ports.RepositoryWrite;
using AlthiraProducts.Products.Application.Commands;
using AlthiraProducts.Products.Application.Diagnostic.Telemetry.ProductTelemetry;
using AlthiraProducts.Products.Application.Mappers.Domain;
using AlthiraProducts.Products.Application.Mappers.Events;
using AlthiraProducts.Products.Application.Mappers.Repository.Write;
using AlthiraProducts.Products.Application.Models.Events;
using AlthiraProducts.Products.Application.Models.Persistence.Write;
using AlthiraProducts.Products.Application.Ports.AzureBlobStorage;
using AlthiraProducts.Products.Application.Ports.ImagesValidator;
using AlthiraProducts.Products.Application.Ports.RepositoryWrite;
using AlthiraProducts.Products.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Products.Application.CommandsHandler;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IOpenTelemetryService _openTelemetryService;
    private readonly IProductRepositoryWrite _productRepositoryWrite;
    private readonly IOutboxEventRepositoryWrite _outboxEventRepositoryWrite;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductImageBlobStorageService _productBlobStorageService;
    private readonly IImageValidatorService _imageValidatorService;

    public CreateProductCommandHandler(
        ILogger<CreateProductCommandHandler> logger,
        IOpenTelemetryService openTelemetryService,
        IProductRepositoryWrite productRepositoryWrite,
        IOutboxEventRepositoryWrite outboxEventRepositoryWrite,
        IUnitOfWork unitOfWork,
        IProductImageBlobStorageService productBlobStorageService,
        IImageValidatorService imageValidatorService)
    {
        _logger = logger;
        _openTelemetryService = openTelemetryService;
        _productRepositoryWrite = productRepositoryWrite;
        _outboxEventRepositoryWrite = outboxEventRepositoryWrite;
        _unitOfWork = unitOfWork;
        _productBlobStorageService = productBlobStorageService;
        _imageValidatorService = imageValidatorService;
    }
    public async Task<Guid> Handle(CreateProductCommand createProductCommand, CancellationToken cancellationToken)
    {
        await _imageValidatorService.ValidateImagesAsync(createProductCommand.CreateProductDto.Images);
        _openTelemetryService.AddStep("Product images validated");

        Product product = Mapper_CreateProdcutDto_Product
            .MapToEntity(createProductCommand.CreateProductDto);
        _openTelemetryService.AddStep("Product bussines validated");

        await _productBlobStorageService.UploadBlobsAsync(
            product.ProductImages.Select(productImage => new BlobModel() 
            { 
                Name = productImage.Name.ToString(),
                Content = productImage.Content,
                ContentType = productImage.ContentType,
            }),
            isTemp:true);

        _openTelemetryService.AddStep("Product images upload in a temp container");
        string traceContext =_openTelemetryService.InjectContextGetString();
        
        ProductWriteModel productWriteModel = Mapper_Product_ProductRepoWrite
            .MapToRepoWrite(product, traceContext);

        _productRepositoryWrite.InsertProduct(productWriteModel);
        _openTelemetryService.AddStep("Product ready for saving in db");

        CreateProductEventCommand createProductEventCommand = Mapper_Product_CreateProductEventCommand.
            MapToEvent(product, nameof(CreateProductCommandHandler));

        OutboxEventWriteModel outboxEventWriteModel = Mapper_Event_OutboxEventWriteModel
            .MapToOutboxEvent(createProductEventCommand, traceContext);

        _outboxEventRepositoryWrite.InsertEvent(outboxEventWriteModel);
        _openTelemetryService.AddStep("Outbox information ready for saving in db");
        _openTelemetryService.AddCreateProductCommandHandlerMetadata(product, createProductEventCommand);

        await _unitOfWork.SaveChangesAsync();
        _openTelemetryService.AddStep("Information saved in database");

        _logger.LogInformation(
            "Product {Sku} created successfully with Id {ProductId}. Associated images: {ImageCount}",
            product.Sku.Id,
            product.Id,
            product.ProductImages.Count());

        return product.Sku.Id;
    }
}