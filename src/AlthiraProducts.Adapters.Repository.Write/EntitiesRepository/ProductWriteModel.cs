namespace AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;

public class ProductWriteModel
{
    public Guid Id { get; set; }
    public Guid Sku { get; set; }
    public int Version { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Status { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryWriteModel Category { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<ProductImageWriteModel> Images { get; set; } = [];
}