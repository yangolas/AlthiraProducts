namespace AlthiraProducts.Products.Domain.ValueObject;

public record class ProductImage
{
    public Guid Name{ get; private set; }
    public int Order { get; private set; }
    public Stream Content { get; private set; } = null!;
    public string ContentType { get; private set; } = null!;

    private static readonly Dictionary<string, string> _dictionaryContentTypesExtensions =
        new()
        {
            ["image/jpeg"] = ".jpg",
            ["image/jpg"] = ".jpg",
            ["image/png"] = ".png"
        };



    private ProductImage() { }

    public static ProductImage Create(
        int order,
        Stream content, 
        string contentType)
    {
        ProductImage productImage = new();
        productImage.Name = Guid.NewGuid();
        productImage.SetContent(content);
        productImage.SetContentType(contentType);
        productImage.SetOrder(order);
        return productImage;
    }


    private void SetContent(Stream content)
    {
        if (content == null || content.Length == 0)
            throw new ArgumentException("Content cannot be empty", nameof(content));

        Content = content;
    }

    private void SetContentType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("ContentType is required", nameof(contentType));

        _dictionaryContentTypesExtensions.TryGetValue(contentType, out string? allowedContentType);
        if (allowedContentType == null)
            throw new ArgumentException($"ContentType '{contentType}' is not allowed", nameof(contentType));
        ContentType = contentType;

    }

    private void SetOrder(int order)
    {
        if (order < 0)
        {
            throw new ArgumentException("ORDER_CANNOT_BE_NEGATIVE", $"The image order must be a non-negative integer. Attempted value: {order}");
        }

        Order = order;
    }
}