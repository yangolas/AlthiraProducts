using AlthiraProducts.BuildingBlocks.Application.EventModel;
using MediatR;

namespace AlthiraProducts.BoundedContext.Products.Application.Models.Events;

public class CreateProductEventCommand : Event, IRequest
{
}