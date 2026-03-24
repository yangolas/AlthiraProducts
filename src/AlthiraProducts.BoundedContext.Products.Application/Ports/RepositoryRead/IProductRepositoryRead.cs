using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;
using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;

namespace AlthiraProducts.BoundedContext.Products.Application.Ports.RepositoryRead;

public interface IProductRepositoryRead : ITransient
{
    Task UpsertWithVersionAsync(ProductReadModel productReadModel);

    Task<(IEnumerable<ProductReadModel> Items, long TotalCount)> GetProductsAsync(
        int page,
        int pageSize,
        string? orderBy = null,
        bool ascending = true);
}
