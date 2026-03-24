using AlthiraProducts.Products.Application.Ports.ImagesValidator;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AlthiraProducts.Adapters.ImagesValidator.Services;

public class ImageValidator : IImageValidatorService
{
    private static readonly string[] AllowedMimeTypes =
    [
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/webp"
    ];

    public async Task ValidateImagesAsync(IEnumerable<IFormFile> images)
    {
        if (images == null)
            throw new ArgumentNullException(nameof(images), "Images collection cannot be null.");

        foreach (var image in images)
        {
            if (image == null)
                throw new ArgumentException("Image cannot be null.");

            if (image.Length == 0)
                throw new ArgumentException($"Image '{image.FileName}' is empty.");

            if (!AllowedMimeTypes.Contains(image.ContentType))
                throw new ArgumentException($"Image '{image.FileName}' has an invalid MIME type '{image.ContentType}'.");

            try
            {
                using var stream = image.OpenReadStream();
                var parsedImage = await Image.LoadAsync<Rgba32>(stream);
                if (parsedImage.Width == 0 || parsedImage.Height == 0)
                    throw new ArgumentException($"Image '{image.FileName}' could not be loaded correctly.");
                stream.Position = 0;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Image '{image.FileName}' is not a valid image. {ex.Message}");
            }
        }
    }

    public async Task<bool> ValidateImagesInBase64Async(IEnumerable<string> images)
    {
        if (images == null)
            throw new ArgumentNullException(nameof(images), "Images collection cannot be null.");

        foreach (var image in images)
        {
            if (string.IsNullOrWhiteSpace(image))
                return false;

            try
            {
                string base64Data = image;
                int commaIndex = base64Data.IndexOf(',');
                if (commaIndex >= 0)
                    base64Data = base64Data[(commaIndex + 1)..];

                base64Data = base64Data.Trim().Replace("\r", "").Replace("\n", "").Replace(" ", "");

                byte[] imageBytes = Convert.FromBase64String(base64Data);

                using var ms = new MemoryStream(imageBytes);
                using var img = await Image.LoadAsync<Rgba32>(ms);
            }
            catch
            {
                return false;
            }
        }

        return true;
    }
}