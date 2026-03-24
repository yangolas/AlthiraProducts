using System.ComponentModel;

namespace AlthiraProducts.BoundedContext.Products.Domain.Enums;

public enum ProductStatus
{
    [Description("In creation")]
    Draft,
    [Description("Sellable")]
    Published,
    [Description("Hidden")]
    Unpublished,
    [Description("Discontinued")]
    Discontinued
}