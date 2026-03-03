using MediatR;

namespace AlthiraProducts.Adapters.MessageBroker.Events.Models.Product;

public class CreateProductEventCommand : Event, IRequest
{
}