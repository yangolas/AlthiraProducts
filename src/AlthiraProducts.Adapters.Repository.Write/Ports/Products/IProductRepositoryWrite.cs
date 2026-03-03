using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;

namespace AlthiraProducts.Adapters.Repository.Write.Ports.Products;

public interface IProductRepositoryWrite : ITransientRepositoryWrite
{
    void InsertProduct(ProductWriteModel productWriteModel);
}
