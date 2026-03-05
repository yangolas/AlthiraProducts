using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;
using AlthiraProducts.Products.Application.Models.Persistence.Read;

namespace AlthiraProducts.Products.Application.Ports.RepositoryRead;

public interface IEventLogRepositoryRead: ITransient
{
    Task UpsertAsync(EventLogReadModel eventLog);
    Task<bool> RegisterIfNotExistsAsync(EventLogReadModel eventLog);
}
