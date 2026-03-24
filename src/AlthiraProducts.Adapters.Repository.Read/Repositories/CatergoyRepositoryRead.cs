using AlthiraProducts.Adapters.Repository.Read.Context;
using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;
using AlthiraProducts.BoundedContext.Products.Application.Ports.RepositoryRead;
using MongoDB.Driver;

namespace AlthiraProducts.Adapters.Repository.Read.Repositories;

public class CatergoyRepositoryRead(ProductReadContext productContext) : ICategoryRepositoryRead
{
    private readonly ProductReadContext _productContext = productContext;

    public async Task<IEnumerable<CategoryReadModel>> GetCategoriesAsync()
    {
        return await _productContext.Categories
         .Find(_ => true)
         .SortBy(c => c.Name)
         .Project(c => new CategoryReadModel
         {
             Id = c.Id,
             Name = c.Name
         })
         .ToListAsync();
    }
}