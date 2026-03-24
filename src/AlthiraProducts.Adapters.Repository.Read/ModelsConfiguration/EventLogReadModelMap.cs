using AlthiraProducts.Adapters.Repository.Read.ModelsConfiguration.Contract;
using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AlthiraProducts.Adapters.Repository.Read.ModelsConfiguration;

public class EventLogReadModelMap : IMongoClassMap
{
    public void Configure()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(EventLogReadModel)))
        {
            BsonClassMap.RegisterClassMap<EventLogReadModel>(cm =>
            {
                cm.AutoMap();

                cm.MapIdProperty(p => p.Id)
                  .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

                cm.MapProperty(p => p.EventName).SetIsRequired(true);

                cm.MapProperty(p => p.Version).SetIsRequired(true);

                cm.MapProperty(p => p.CreatedAt)
                  .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));

                cm.MapProperty(p => p.Source).SetIgnoreIfNull(true);
            });
        }
    }
}