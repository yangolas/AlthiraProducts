using AlthiraProducts.Adapters.Repository.Read.Models;

namespace AlthiraProducts.Adapters.Repository.Read.Ports;

public interface IEventLogRepositoryRead: ITransientRepositoryRead
{
    Task UpsertAsync(EventLogReadModel eventLog);
    Task<bool> RegisterIfNotExistsAsync(EventLogReadModel eventLog);
}
