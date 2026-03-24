using AlthiraProducts.Products.Application.Models.Dtos;
using AlthiraProducts.Products.Domain.Enums;

namespace AlthiraProducts.Products.Application.Models.Dtos.Events;

public class CreateProductEventDto
{
    public Guid Id { get; set; }
    public Guid Sku { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public ProductStatus ProductStatus { get; set; }
    public CategoryDto Category { get; set; } = null!;
    public string? Description { get; set; }
    public IEnumerable<string> Images { get; set; } = null!;
}