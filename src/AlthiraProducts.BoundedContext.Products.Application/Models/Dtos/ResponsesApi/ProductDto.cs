using AlthiraProducts.BoundedContext.Products.Domain.Enums;

namespace AlthiraProducts.BoundedContext.Products.Application.Models.Dtos.ResponsesApi;

public class ProductDto
{
    public Guid Sku { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public ProductStatus ProductStatus { get; set; }
    public CategoryDto Category{ get; set; } = null!;
    public string? Description { get; set; }
    public IEnumerable<string> Images { get; set; } = null!;
}