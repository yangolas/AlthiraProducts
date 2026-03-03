using AlthiraProducts.Adapters.Repository.Write.Context;
using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;
using AlthiraProducts.Adapters.Repository.Write.Enums;
using AlthiraProducts.Adapters.Repository.Write.Ports.Products;
using Microsoft.EntityFrameworkCore;

namespace AlthiraProducts.Adapters.Repository.Write.Repositories;

public class OutboxEventRepository(ProductWriteContext _productContext) : IOutboxEventRepositoryWrite
{
    public async Task<IEnumerable<OutboxEventWriteModel>> GetPendingEventsAsync(int batchSize)
    {
        DateTime now = DateTime.UtcNow;

        return await _productContext.OutboxEvents
            .Where(outboxEvent =>
                outboxEvent.Status == OutboxStatus.Pending 
                && (outboxEvent.NextRetryAt == null 
                    || outboxEvent.NextRetryAt <= now))
            .OrderBy(outboxEvent => outboxEvent.CreatedAt)
            .Take(batchSize)
            .ToListAsync();
    }

    public void InsertEvent(OutboxEventWriteModel outboxEventWriteModel)
    {
        _productContext.OutboxEvents.Add(outboxEventWriteModel);
    }
}