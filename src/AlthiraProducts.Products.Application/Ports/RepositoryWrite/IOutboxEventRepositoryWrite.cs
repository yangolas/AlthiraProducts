using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;
using AlthiraProducts.Products.Application.Models.Persistence.Write;

namespace AlthiraProducts.Products.Application.Ports.RepositoryWrite;

public interface IOutboxEventRepositoryWrite : IScoped
{
    Task<IEnumerable<OutboxEventWriteModel>> GetPendingEventsAsync(int batchSize);

    void InsertEvent(OutboxEventWriteModel outboxEventWriteModel);
}