using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Write;
using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;

namespace AlthiraProducts.BoundedContext.Products.Application.Ports.RepositoryWrite;

public interface IImageRepositoryWrite : ITransient
{
    Task<IEnumerable<ProductImageWriteModel>> GetProductsWithPendingImagesAsync(int batchsize);
}