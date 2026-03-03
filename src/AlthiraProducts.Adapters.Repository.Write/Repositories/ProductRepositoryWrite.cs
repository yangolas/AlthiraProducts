using AlthiraProducts.Adapters.Repository.Write.Context;
using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;
using AlthiraProducts.Adapters.Repository.Write.Ports.Products;

namespace AlthiraProducts.Adapters.Repository.Write.Repositories;

public class ProductRepositoryWrite(ProductWriteContext productContext) : IProductRepositoryWrite
{
    private readonly ProductWriteContext _productContext = productContext;

    public void InsertProduct(ProductWriteModel productWriteModel)
    {
        _productContext.Products.Add(productWriteModel);
    }
}