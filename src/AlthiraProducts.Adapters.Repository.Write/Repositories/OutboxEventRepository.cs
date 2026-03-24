using AlthiraProducts.Adapters.Repository.Write.Context;
using AlthiraProducts.Products.Application.Models.Persistence.Write;
using AlthiraProducts.Products.Application.Ports.RepositoryWrite;
using AlthiraProducts.Products.Application.Ports.RepositoryWrite.Enums;
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
            .OrderBy(outboxEvent => outboxEvent.InsertedAt)
            .Take(batchSize)
            .ToListAsync();
    }

    public void InsertEvent(OutboxEventWriteModel outboxEventWriteModel)
    {
        _productContext.OutboxEvents.Add(outboxEventWriteModel);
    }
}