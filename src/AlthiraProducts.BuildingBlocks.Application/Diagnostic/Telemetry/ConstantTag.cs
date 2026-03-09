namespace AlthiraProducts.BuildingBlocks.Application.Diagnostic.Telemetry;

public static class ConstantTag
{
    public readonly static string MicroserviceName = "althiraproduct";
    public readonly static string EntityProduct = "product";
    public readonly static string EntityEvent = "event";
    public readonly static string EntityCategory = "category";
    public readonly static string EntityImage = "image";
    public readonly static string EntityMessageBroker = "message_broker";
    public readonly static string EntityOutbox = "outbox";
    public readonly static string EntityProductBlobStorage = "product_blob_storage";
    public readonly static string Id = "id";
    public readonly static string Version = "version";
    public readonly static string Retry = "retry";
    public readonly static string NextRetry = "next_retry";
    public readonly static string MaxRetry = "max_retry";
    public readonly static string Name = "name";
    public readonly static string Sku = "sku";
    public readonly static string Count = "count";
    //public readonly static string NextLevel = "next_level";
    public readonly static string RoutingKey = "routing_key";
    public readonly static string NextRoutingKey = "next_routing_key";
    public readonly static string Exchange = "exchange";
    public readonly static string Status = "status";
    public readonly static string CreatedAt = "created_at";
    public readonly static string InQueueAt = "in_queue_at";
    public readonly static string ProcessedAt = "in_queue_at";
}