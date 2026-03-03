using Microsoft.AspNetCore.Http;

namespace AlthiraProducts.Adapters.ImagesValidator.Ports;

public interface IImageValidatorService: ITransientImageValidator
{
    Task ValidateImagesAsync(IEnumerable<IFormFile> images);
    Task<bool> ValidateImagesInBase64Async(IEnumerable<string> images);
}
