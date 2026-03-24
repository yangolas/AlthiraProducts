using AlthiraProducts.Adapters.Repository.Read.ModelsConfiguration.Contract;
using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Read;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AlthiraProducts.Adapters.Repository.Read.ModelsConfiguration;

internal class ProductReadModelMap : IMongoClassMap
{
    public void Configure()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(ProductReadModel)))
        {
            BsonClassMap.RegisterClassMap<ProductReadModel>(cm =>
            {
                cm.AutoMap();

                cm.MapIdProperty(p => p.Id)
                  .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

                cm.MapProperty(p => p.Sku)
                  .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                
                cm.MapProperty(p => p.Name).SetIsRequired(true);

                cm.MapProperty(p => p.Version).SetIsRequired(true);

                cm.MapProperty(p => p.Price)
                  .SetSerializer(new DecimalSerializer(BsonType.Decimal128));

                cm.MapProperty(p => p.Status).SetIsRequired(true);

                cm.MapProperty(p => p.Description).SetIgnoreIfNull(true);

                cm.MapProperty(p => p.Images)
                  .SetDefaultValue(() => Enumerable.Empty<string>());

            });
        }
    }
}
