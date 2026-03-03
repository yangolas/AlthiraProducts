using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;

namespace AlthiraProducts.Adapters.Repository.Write.Ports.Products;

public interface IImageRepositoryWrite : ITransientRepositoryWrite
{
    Task<IEnumerable<ProductImageWriteModel>> GetProductsWithPendingImagesAsync(int batchsize);
}