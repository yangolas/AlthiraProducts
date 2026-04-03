using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;
using AlthiraProducts.Products.Application.Models.Persistence.Write;

namespace AlthiraProducts.Products.Application.Ports.RepositoryWrite;

public interface IProductRepositoryWrite : IScoped
{
    void InsertProduct(ProductWriteModel productWriteModel);
}
