using AlthiraProducts.Adapters.Repository.Read.Context;
using AlthiraProducts.Products.Application.Models.Persistence.Read;
using AlthiraProducts.Products.Application.Ports.RepositoryRead;
using MongoDB.Driver;

namespace AlthiraProducts.Adapters.Repository.Read.Repositories;

public class ProductRepositoryRead(ProductReadContext _productContext) : IProductRepositoryRead
{

    public async Task<(IEnumerable<ProductReadModel> Items, long TotalCount)> GetProductsAsync(
        int page,
        int pageSize,
        string? orderBy = null,
        bool ascending = true) 
    {
        FilterDefinition<ProductReadModel> filter = Builders<ProductReadModel>.Filter.Empty;

        var totalCount = await _productContext.Products.CountDocumentsAsync(filter);

        var query = _productContext.Products.Find(filter);

        if (!string.IsNullOrEmpty(orderBy))
        {
            query = ascending
                ? query.Sort(Builders<ProductReadModel>.Sort.Ascending(orderBy))
                : query.Sort(Builders<ProductReadModel>.Sort.Descending(orderBy));
        }

        var items = await query
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task UpsertWithVersionAsync(ProductReadModel product)
    {
        var filter = Builders<ProductReadModel>.Filter.And(
            Builders<ProductReadModel>.Filter.Eq(x => x.Id, product.Id),
            Builders<ProductReadModel>.Filter.Lt(x => x.Version, product.Version)
        );

        await _productContext.Products.ReplaceOneAsync(
            filter,
            product,
            new ReplaceOptions { IsUpsert = true }
        );
    }
}