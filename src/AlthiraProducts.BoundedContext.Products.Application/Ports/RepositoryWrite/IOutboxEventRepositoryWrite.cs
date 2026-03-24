using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Write;
using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;

namespace AlthiraProducts.BoundedContext.Products.Application.Ports.RepositoryWrite;

public interface IOutboxEventRepositoryWrite : ITransient
{
    Task<IEnumerable<OutboxEventWriteModel>> GetPendingEventsAsync(int batchSize);

    void InsertEvent(OutboxEventWriteModel outboxEventWriteModel);
}