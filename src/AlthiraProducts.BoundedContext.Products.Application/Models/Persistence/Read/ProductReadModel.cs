namespace AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;

public class ProductReadModel
{
    public Guid Id { get; set; }

    public Guid Sku { get; set; }

    public string Name { get; set; } = null!;

    public int Version { get; set; }

    public decimal Price { get; set; }

    public int Status{ get; set; }

    public CategoryReadModel Category { get; set; } = null!;

    public string? Description { get; set; }

    public IEnumerable<string> Images { get; set; } = [];
}
