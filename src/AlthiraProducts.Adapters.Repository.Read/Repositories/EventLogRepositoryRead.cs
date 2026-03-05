using AlthiraProducts.Adapters.Repository.Read.Context;
using AlthiraProducts.Products.Application.Models.Persistence.Read;
using AlthiraProducts.Products.Application.Ports.RepositoryRead;
using MongoDB.Driver;

namespace AlthiraProducts.Adapters.Repository.Read.Repositories;

public class EventLogRepositoryRead(ProductReadContext productContext) : IEventLogRepositoryRead
{
    private readonly ProductReadContext _productContext = productContext;

    public async Task UpsertAsync(EventLogReadModel eventLog)
    {
        await _productContext.EventLogs.ReplaceOneAsync(
            filter: x => x.Id == eventLog.Id,
            replacement: eventLog,
            options: new ReplaceOptions { IsUpsert = true }
        );
    }
    public async Task<bool> RegisterIfNotExistsAsync(EventLogReadModel eventLog)
    {
        try
        {
            await _productContext.EventLogs.InsertOneAsync(eventLog);
            return true;
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}