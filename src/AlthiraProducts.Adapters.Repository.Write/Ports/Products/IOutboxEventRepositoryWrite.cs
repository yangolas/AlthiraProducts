using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;

namespace AlthiraProducts.Adapters.Repository.Write.Ports.Products;

public interface IOutboxEventRepositoryWrite : ITransientRepositoryWrite
{
    Task<IEnumerable<OutboxEventWriteModel>> GetPendingEventsAsync(int batchSize);

    void InsertEvent(OutboxEventWriteModel outboxEventWriteModel);
}