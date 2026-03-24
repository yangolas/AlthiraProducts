using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;
using AlthiraProducts.Products.Application.Models.Persistence.Write;

namespace AlthiraProducts.Products.Application.Ports.RepositoryWrite;

public interface IImageRepositoryWrite : ITransient
{
    Task<IEnumerable<ProductImageWriteModel>> GetProductsWithPendingImagesAsync(int batchsize);
}