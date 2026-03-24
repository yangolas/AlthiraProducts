namespace AlthiraProducts.BoundedContext.Products.Domain.ValueObject;

public readonly record struct Sku
{
    private const string INVALID_SKU = "SKU cannot be null or empty.";
    public Guid Id { get; init; }

    public Sku(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(INVALID_SKU, nameof(value));
        }
        Id = value;
    }

}