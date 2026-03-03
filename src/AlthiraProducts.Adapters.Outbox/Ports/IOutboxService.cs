namespace AlthiraProducts.Adapters.Outbox.Ports;

public interface IOutboxService
{
    Task ProcessOutboxAsync();
}