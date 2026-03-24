namespace AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;

public class EventLogReadModel
{
    public Guid Id { get; set; }
    public string EventName { get; set; } = null!;
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Source { get; set; }
}
