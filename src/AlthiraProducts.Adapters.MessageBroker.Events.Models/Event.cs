namespace AlthiraProducts.Adapters.MessageBroker.Events.Models;

public class Event
{
    public Guid Id { get; set; }

    public string EventName { get; set; } = null!;

    public string Payload { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    
    public int Version { get; set; }
    
    public string? Source { get; set; }
}