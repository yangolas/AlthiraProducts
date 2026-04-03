using AlthiraProducts.Adapters.Repository.Write.Context;
using AlthiraProducts.Adapters.Repository.Write.SeedData;
using AlthiraProducts.Adapters.Repository.Write.Settings;
using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AlthiraProducts.Adapters.Repository.Write;

public static class ServiceRegister
{
    public static void AddRepositoryContextWrite(
        this IServiceCollection serviceCollection,
        DatabaseWriteSettings databaseWriteSettings)
    {
        serviceCollection.AddDbContext<ProductWriteContext>(service =>
        {
            service.UseSqlServer(databaseWriteSettings.ConnectionString);
            service.EnableSensitiveDataLogging(false);
            service.ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted));
        }
        );
    }

    public static void AddRepositoryWrite(
        this IServiceCollection serviceCollection,
        string assemblyRepositoryWrite)
    {
        Assembly assembly = Assembly.Load(assemblyRepositoryWrite);
        serviceCollection.Scan(scan =>
            scan.FromAssemblies(assembly)
            .AddClasses(clases =>
                clases.AssignableToAny(typeof(IScoped))
            )
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );
    }

    public static async Task ApplyMigrationsDbWriteAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        ProductWriteContext context = scope.ServiceProvider.GetRequiredService<ProductWriteContext>();
        await context.Database.MigrateAsync();
        await SeedDataCategoryWrite.AddSeedData(context);
    }
}
