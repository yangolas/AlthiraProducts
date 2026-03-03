using AlthiraProducts.Adapters.MessageBroker.Events.Models.Product;
using AlthiraProducts.Adapters.Repository.Read.Models;
using AlthiraProducts.Adapters.Repository.Read.Ports;
using AlthiraProducts.Products.Application.Dtos.Events;
using AlthiraProducts.Products.Application.Mappers.Repository.Read;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AlthiraProducts.Products.Application.EventsHandlers;

public class CreateProductEventCommandHandler : IRequestHandler<CreateProductEventCommand>
{
    private readonly ILogger<CreateProductEventCommandHandler> _logger;
    private readonly IProductRepositoryRead _productRepositoryRead;
    private readonly IEventLogRepositoryRead _eventLogRepositoryRead;

    public CreateProductEventCommandHandler(
         ILogger<CreateProductEventCommandHandler> logger,
         IProductRepositoryRead productRepositoryRead,
         IEventLogRepositoryRead eventLogRepositoryRead)
    {
        _logger = logger;
        _productRepositoryRead = productRepositoryRead;
        _eventLogRepositoryRead = eventLogRepositoryRead;
    }
    public async Task Handle(CreateProductEventCommand request, CancellationToken cancellationToken)
    {
        EventLogReadModel @event = request.MapToRepoRead();
        CreateProductEventDto createProductEventDto = JsonSerializer.Deserialize<CreateProductEventDto>(request.Payload)!;
        ProductReadModel productReadModel = createProductEventDto.MapToRepoRead();
        
        await _productRepositoryRead.UpsertWithVersionAsync(productReadModel);
        await _eventLogRepositoryRead.UpsertAsync(@event);
    }
}