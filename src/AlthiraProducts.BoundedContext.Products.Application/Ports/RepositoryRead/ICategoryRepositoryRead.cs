using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;
using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;

namespace AlthiraProducts.BoundedContext.Products.Application.Ports.RepositoryRead;

public interface ICategoryRepositoryRead : ITransient
{
    Task<IEnumerable<CategoryReadModel>> GetCategoriesAsync();
}