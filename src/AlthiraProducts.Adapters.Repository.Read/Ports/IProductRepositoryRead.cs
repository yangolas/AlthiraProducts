using AlthiraProducts.Adapters.Repository.Read.Models;

namespace AlthiraProducts.Adapters.Repository.Read.Ports;

public interface IProductRepositoryRead : ITransientRepositoryRead
{
    Task UpsertWithVersionAsync(ProductReadModel productReadModel);

    Task<(IEnumerable<ProductReadModel> Items, long TotalCount)> GetProductsAsync(
        int page,
        int pageSize,
        string? orderBy = null,
        bool ascending = true);
}
