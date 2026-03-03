using AlthiraProducts.Adapters.Repository.Read.Models;
using AlthiraProducts.Adapters.Repository.Read.ModelsConfiguration.Contract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AlthiraProducts.Adapters.Repository.Read.ModelsConfiguration;

public class CategoryReadModelMap : IMongoClassMap
{
    public void Configure()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(CategoryReadModel)))
        {
            BsonClassMap.RegisterClassMap<CategoryReadModel>(cm =>
            {
                cm.AutoMap();

                cm.MapIdProperty(p => p.Id)
                  .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

                cm.MapProperty(p => p.Name).SetElementName("name");

                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}