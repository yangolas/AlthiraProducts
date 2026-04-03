using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;
using AlthiraProducts.Products.Application.Models.Persistence.Read;

namespace AlthiraProducts.Products.Application.Ports.RepositoryRead;

public interface ICategoryRepositoryRead : IScoped
{
    Task<IEnumerable<CategoryReadModel>> GetCategoriesAsync();
}