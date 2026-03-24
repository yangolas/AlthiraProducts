using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Write;
using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;

namespace AlthiraProducts.BoundedContext.Products.Application.Ports.RepositoryWrite;

public interface IProductRepositoryWrite : ITransient
{
    void InsertProduct(ProductWriteModel productWriteModel);
}
