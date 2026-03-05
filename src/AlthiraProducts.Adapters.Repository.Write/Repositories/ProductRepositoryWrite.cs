using AlthiraProducts.Adapters.Repository.Write.Context;
using AlthiraProducts.Products.Application.Models.Persistence.Write;
using AlthiraProducts.Products.Application.Ports.RepositoryWrite;

namespace AlthiraProducts.Adapters.Repository.Write.Repositories;

public class ProductRepositoryWrite(ProductWriteContext productContext) : IProductRepositoryWrite
{
    private readonly ProductWriteContext _productContext = productContext;

    public void InsertProduct(ProductWriteModel productWriteModel)
    {
        _productContext.Products.Add(productWriteModel);
    }
}