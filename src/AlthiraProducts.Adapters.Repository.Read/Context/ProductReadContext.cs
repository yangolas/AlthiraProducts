using AlthiraProducts.Adapters.Repository.Read.ModelsConfiguration.Contract;
using AlthiraProducts.Adapters.Repository.Read.Settings;
using AlthiraProducts.Products.Application.Models.Persistence.Read;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace AlthiraProducts.Adapters.Repository.Read.Context;

public class ProductReadContext
{
    private readonly IMongoDatabase _database;
    public IMongoCollection<ProductReadModel> Products { get; set; } = null!;
    public IMongoCollection<EventLogReadModel> EventLogs { get; set; } = null!;
    public IMongoCollection<CategoryReadModel> Categories { get; set; } = null!;
    private static bool _conventionsRegistered = false;
    private static void RegisterMappings()
    {
        if (!_conventionsRegistered)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true)
            };
            ConventionRegistry.Register("AlthiraConventions", pack, t => true);
            _conventionsRegistered = true;
        }

        var mappingTypes = typeof(ProductReadContext).Assembly.GetTypes()
            .Where(type => typeof(IMongoClassMap).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var type in mappingTypes)
        {
            var mapping = (IMongoClassMap)Activator.CreateInstance(type)!;
            mapping.Configure();
        }
    }

    public ProductReadContext(DatabaseReadSettings databaseReadSettings)
    {
        RegisterMappings();
        MongoClient mongoClient = new(databaseReadSettings.ConnectionString);
        _database = mongoClient.GetDatabase(databaseReadSettings.DatabaseName);
        Products = _database.GetCollection<ProductReadModel>(nameof(Products).ToLower());
        EventLogs = _database.GetCollection<EventLogReadModel>(nameof(EventLogs).ToLower());
        Categories = _database.GetCollection<CategoryReadModel>(nameof(Categories).ToLower());
    }
}