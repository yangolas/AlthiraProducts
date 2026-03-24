using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;
using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;

namespace AlthiraProducts.BoundedContext.Products.Application.Ports.RepositoryRead;

public interface IEventLogRepositoryRead: ITransient
{
    Task UpsertAsync(EventLogReadModel eventLog);
    Task<bool> RegisterIfNotExistsAsync(EventLogReadModel eventLog);
}
