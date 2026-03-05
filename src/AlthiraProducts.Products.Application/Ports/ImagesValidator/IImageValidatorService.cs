using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;
using Microsoft.AspNetCore.Http;

namespace AlthiraProducts.Products.Application.Ports.ImagesValidator;

public interface IImageValidatorService: ITransient
{
    Task ValidateImagesAsync(IEnumerable<IFormFile> images);
    Task<bool> ValidateImagesInBase64Async(IEnumerable<string> images);
}
