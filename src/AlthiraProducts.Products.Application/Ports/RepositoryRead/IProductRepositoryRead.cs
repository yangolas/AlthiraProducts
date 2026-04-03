using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;
using AlthiraProducts.Products.Application.Models.Persistence.Read;

namespace AlthiraProducts.Products.Application.Ports.RepositoryRead;

public interface IProductRepositoryRead : IScoped
{
    Task UpsertWithVersionAsync(ProductReadModel productReadModel);

    Task<(IEnumerable<ProductReadModel> Items, long TotalCount)> GetProductsAsync(
        int page,
        int pageSize,
        string? orderBy = null,
        bool ascending = true);
}
