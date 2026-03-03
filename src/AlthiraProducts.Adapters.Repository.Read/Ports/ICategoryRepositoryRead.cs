using AlthiraProducts.Adapters.Repository.Read.Models;

namespace AlthiraProducts.Adapters.Repository.Read.Ports;

public interface ICategoryRepositoryRead : ITransientRepositoryRead
{
    Task<IEnumerable<CategoryReadModel>> GetCategoriesAsync();
}