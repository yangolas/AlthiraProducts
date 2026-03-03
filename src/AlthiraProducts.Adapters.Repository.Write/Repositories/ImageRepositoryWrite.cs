using AlthiraProducts.Adapters.Repository.Write.Context;
using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;
using AlthiraProducts.Adapters.Repository.Write.Enums;
using AlthiraProducts.Adapters.Repository.Write.Ports.Products;
using Microsoft.EntityFrameworkCore;

namespace AlthiraProducts.Adapters.Repository.Write.Repositories;

public class ImageRepositoryWrite(ProductWriteContext _productContext) : IImageRepositoryWrite
{
    public async Task<IEnumerable<ProductImageWriteModel>> GetProductsWithPendingImagesAsync(int batchSize)
    {
        DateTime now = DateTime.UtcNow;

        return await _productContext.Images
            .Where(image =>
                image.Status == ImageStatus.Pending
                && (image.NextRetryAt == null
                    || image.NextRetryAt <= now))
            .OrderBy(image => image.CreatedAt)
            .Take(batchSize)
            .ToListAsync();
    }
}