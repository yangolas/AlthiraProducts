namespace AlthiraProducts.BoundedContext.Products.Domain.Entities;

public class Category
{
    private const string INVALID_CATEGORY_ID = "CategoryId cannot be empty.";
    public Guid Id { get; }
    public string Name { get; init; } = null!;

    public Category(Guid id,string departmentName)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(INVALID_CATEGORY_ID, nameof(id));
        Id = id;
        Name = departmentName;
    }
}