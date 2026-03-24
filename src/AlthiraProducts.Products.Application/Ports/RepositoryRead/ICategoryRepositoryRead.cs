using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;
using AlthiraProducts.Products.Application.Models.Persistence.Read;

namespace AlthiraProducts.Products.Application.Ports.RepositoryRead;

public interface ICategoryRepositoryRead : ITransient
{
    Task<IEnumerable<CategoryReadModel>> GetCategoriesAsync();
}