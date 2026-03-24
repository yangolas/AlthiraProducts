using AlthiraProducts.Adapters.Repository.Write.Context;
using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Write;
using Microsoft.EntityFrameworkCore;

namespace AlthiraProducts.Adapters.Repository.Write.SeedData;

internal class SeedDataCategoryWrite
{
    internal async static Task AddSeedData(ProductWriteContext context) 
    {
        var seedCategories = new List<CategoryWriteModel>
        {
            new CategoryWriteModel { Id = Guid.Parse("9973C1AA-60E9-49D5-A4C5-00C56BABD794"), DepartmentName = "Mundo Infantil" },
            new CategoryWriteModel { Id = Guid.Parse("FBEAE1CE-D2E7-483C-9FCB-6665E56A18C3"), DepartmentName = "Hogar y Confort" },
            new CategoryWriteModel { Id = Guid.Parse("2C67B224-D806-4211-A1C4-F633EDBFFD63"), DepartmentName = "Entretenimiento y Ocio" }
        };

        foreach (var cat in seedCategories)
        {
            bool exists = await context.Categories.AnyAsync(c => c.Id == cat.Id);
            if (!exists)
            {
                context.Categories.Add(cat);
            }
        }

        await context.SaveChangesAsync();
    }
}
