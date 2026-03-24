using AlthiraProducts.BoundedContext.Products.Domain.Enums;
using AlthiraProducts.BoundedContext.Products.Domain.ValueObject;

namespace AlthiraProducts.BoundedContext.Products.Domain.Entities;

public class Product
{
    private const string INVALID_NAME = "Name cannot be null or empty.";
    private const string INVALID_PRICE = "Price cannot be negative.";
    private const string INVALID_DESCRIPTION = "Description cannot exceed {0} characters.";
    private const string INVALID_IMAGE = "One or more images are not valid Base64 images";
    private const string IVALID_PRODUCT_ID = "Product Id is Empty";
    private const string IVALID_SKU_STATUS_DRAFT = "SKU can only be changed while product is in Draft state.";
    private const string INVALID_PUBLISH_PRICE = "Product must have a positive price to be published.";
    private const string INVALID_PUBLISH_STATE_IN_DRAFT_UNPUBLISH = "Only Draft or Unpublished products can be published.";
    private const string INVALID_UNPUBLISH_STATE_DIFFERENT_PUBLISH = "Only Published products can be unpublished.";
    private const string INVALID_DISCONTINUED_STATE_IN_CREATION_PRODUCT = "Only Published products can be unpublished.";
    private const string INVALID_DRAFT_STATE= "Only new products can stay in the draft state.";
    private const int MAX_DESCRIPTION_LENGTH = 1000000;
    private const int MAX_NUMBER_IMAGES = 9;

    public Guid Id { get; private set; } 
    public Sku Sku { get; private set; }
    public int Version { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public ProductStatus Status { get; private set; }
    public Category Category { get; private set; } = null!;
    public string? Description { get; private set; }
    public IEnumerable<ProductImage> ProductImages { get; private set; } = null!;

    private Product() {}

    public static Product Create(
        Sku sku,
        Category category,
        ProductStatus productStatus,
        string name,
        decimal price,
        string? description,
        IEnumerable<ProductImage> productImages) 
    {
        Product product = new ();
        product.SetId(Guid.NewGuid());
        product.SetSku(sku);
        product.SetProcessStatusOnCreateProduct(productStatus);
        product.SetName(name);
        product.SetPrice(price);
        product.SetCategory(category);
        product.SetDescription(description);
        product.SetImages(productImages);
        product.Version = 1;
        return product;
    }

    public void SetId(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidOperationException(IVALID_PRODUCT_ID);
        Id = id;
    }

    public void SetSku(Sku sku)
    {
        if (Status != ProductStatus.Draft)
            throw new InvalidOperationException(IVALID_SKU_STATUS_DRAFT);
        Sku = sku;
    }

    public void SetCategory(Category categoryId)
    {
        Category = categoryId;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(INVALID_NAME, nameof(name));
        Name = name;
    }

    public void SetPrice(decimal price)
    {
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), INVALID_PRICE);
        Price = price;
    }

    public void SetDescription(string? description)
    {
        if (description == null) 
            return;

        if (description.IsWhiteSpace() 
            && description == string.Empty 
            && description?.Length > MAX_DESCRIPTION_LENGTH)
            
            throw new ArgumentException(message: string.Format(INVALID_DESCRIPTION, 
                nameof(description), MAX_DESCRIPTION_LENGTH));
        
        Description = description;
    }

    public void SetImages(IEnumerable<ProductImage> productImages)
    {
        if (productImages.Count() > MAX_NUMBER_IMAGES)
            throw new InvalidOperationException(INVALID_IMAGE);

        ProductImages = productImages;
    }


    public void SetProcessStatusOnCreateProduct(ProductStatus productStatus) 
    {
        if (productStatus == ProductStatus.Discontinued)
            throw new InvalidOperationException(INVALID_DISCONTINUED_STATE_IN_CREATION_PRODUCT);
        Status = productStatus;
    }
    
    public void SetStatusPublish()
    {
        if (Status == ProductStatus.Discontinued)
            throw new InvalidOperationException(INVALID_PUBLISH_STATE_IN_DRAFT_UNPUBLISH);
        if (Price <= 0)
            throw new InvalidOperationException(INVALID_PUBLISH_PRICE);      
        Status = ProductStatus.Published;
    }

    public void SetStatusUnpublish()
    {   
        if (Status == ProductStatus.Unpublished) 
            return;
        if (Status != ProductStatus.Published )
            throw new InvalidOperationException(INVALID_UNPUBLISH_STATE_DIFFERENT_PUBLISH);
        Status = ProductStatus.Unpublished;
    }

    public void SetStatusDiscontinue()
    {
        Status = ProductStatus.Discontinued;
    }
}