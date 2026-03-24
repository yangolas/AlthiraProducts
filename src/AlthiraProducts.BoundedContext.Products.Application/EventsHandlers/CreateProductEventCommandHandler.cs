using AlthiraProducts.BoundedContext.Products.Application.Mappers.Repository.Read;
using AlthiraProducts.BoundedContext.Products.Application.Models.Dtos.Events;
using AlthiraProducts.BoundedContext.Products.Application.Models.Events;
using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;
using AlthiraProducts.BoundedContext.Products.Application.Ports.RepositoryRead;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AlthiraProducts.BoundedContext.Products.Application.EventsHandlers;

public class CreateProductEventCommandHandler : IRequestHandler<CreateProductEventCommand>
{
    private readonly ILogger<CreateProductEventCommandHandler> _logger;
    private readonly IOpenTelemetryService _openTelemetryService; 
    private readonly IProductRepositoryRead _productRepositoryRead;
    private readonly IEventLogRepositoryRead _eventLogRepositoryRead;

    public CreateProductEventCommandHandler(
         ILogger<CreateProductEventCommandHandler> logger,
         IOpenTelemetryService openTelemetryService,
         IProductRepositoryRead productRepositoryRead,
         IEventLogRepositoryRead eventLogRepositoryRead)
    {
        _logger = logger;
        _openTelemetryService = openTelemetryService;
        _productRepositoryRead = productRepositoryRead;
        _eventLogRepositoryRead = eventLogRepositoryRead;
    }
    public async Task Handle(CreateProductEventCommand request, CancellationToken cancellationToken)
    {
        _openTelemetryService.AddStep("Starting CreateProductEventCommandHandler");

        EventLogReadModel @event = request.MapToRepoRead();
        CreateProductEventDto createProductEventDto = JsonSerializer.Deserialize<CreateProductEventDto>(request.Payload)!;
        ProductReadModel productReadModel = createProductEventDto.MapToRepoRead();

        _logger.LogInformation("Processing product creation. ProductId: {ProductId}, EventId: {EventId}",
        productReadModel.Id, @event.Id);

        await _productRepositoryRead.UpsertWithVersionAsync(productReadModel);
        await _eventLogRepositoryRead.UpsertAsync(@event);

        _openTelemetryService.AddStep("Command Handler finished successfully");
    }
}