using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;
using AlthiraProducts.Products.Application.Models.Persistence.Write;

namespace AlthiraProducts.Products.Application.Ports.RepositoryWrite;

public interface IOutboxEventRepositoryWrite : ITransient
{
    Task<IEnumerable<OutboxEventWriteModel>> GetPendingEventsAsync(int batchSize);

    void InsertEvent(OutboxEventWriteModel outboxEventWriteModel);
}