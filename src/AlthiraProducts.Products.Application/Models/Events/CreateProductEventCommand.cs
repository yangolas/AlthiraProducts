using AlthiraProducts.BuildingBlocks.Application.EventModel;
using MediatR;

namespace AlthiraProducts.Products.Application.Models.Events;

public class CreateProductEventCommand : Event, IRequest
{
}