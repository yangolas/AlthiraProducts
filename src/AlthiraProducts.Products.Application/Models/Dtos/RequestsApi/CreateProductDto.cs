using AlthiraProducts.Products.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace AlthiraProducts.Products.Application.Models.Dtos.RequestsApi;

public class CreateProductDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public ProductStatus ProductStatus { get; set; }
    public CategoryDto Category { get; set; } = null!;
    public string? Description { get; set; }
    public List<IFormFile> Images { get; set; } = null!;
}
