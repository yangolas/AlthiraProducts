using AlthiraProducts.Adapters.Repository.Write.Context;
using AlthiraProducts.Products.Application.Models.Persistence.Write;
using AlthiraProducts.Products.Application.Ports.RepositoryWrite;
using AlthiraProducts.Products.Application.Ports.RepositoryWrite.Enums;
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
            .OrderBy(image => image.InsertedAt)
            .Take(batchSize)
            .ToListAsync();
    }
}