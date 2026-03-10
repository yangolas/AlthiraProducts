using AlthiraProducts.Adapters.Repository.Read.Context;
using AlthiraProducts.Products.Application.Models.Persistence.Read;
using Microsoft.Extensions.DependencyInjection;

namespace AlthiraProducts.Adapters.Repository.Read.SeedData;

internal static class SeedDataCategoryRead
{
    internal static async Task AddSeedDataAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProductReadContext>();

        var collection = context.Categories;

        var filter = MongoDB.Driver.Builders<CategoryReadModel>.Filter.Empty;
        var count = await collection.CountDocumentsAsync(filter);

        if (count == 0)
        {
            var seedCategories = new List<CategoryReadModel>
        {
            new CategoryReadModel { Id = Guid.Parse("9973c1aa-60e9-49d5-a4c5-00c56babd794"), Name = "Mundo Infantil" },
            new CategoryReadModel { Id = Guid.Parse("fbeae1ce-d2e7-483c-9fcb-6665e56a18c3"), Name = "Hogar y Confort" },
            new CategoryReadModel { Id = Guid.Parse("2c67b224-d806-4211-a1c4-f633edbffd63"), Name = "Entretenimiento y Ocio" }
        };

            await collection.InsertManyAsync(seedCategories);
            Console.WriteLine("🌱 MongoDB: Semilla plantada con éxito.");
        }
    }
}
