namespace AlthiraProducts.Products.Application.Ports.RepositoryWrite.Enums;

public enum OutboxStatus
{
    Pending = 0,
    Processing = 1,
    Processed = 2,
    Failed = 3
}