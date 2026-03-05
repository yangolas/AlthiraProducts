namespace AlthiraProducts.BuildingBlocks.Application.Ports.Outbox;

public interface IOutboxService
{
    Task ProcessOutboxAsync();
}