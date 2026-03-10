using AlthiraProducts.Adapters.Repository.Read.Services;
using AlthiraProducts.Adapters.Repository.Write;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AlthiraProducts.Main.Process;

internal static class DbMigrationProcess
{
    internal static async Task Start(AlthiraProductsSettings config)
    {
        IServiceCollection servicesMigration = new ServiceCollection();
        servicesMigration.AddRepositoryContextWrite(config.DatabaseWrite);
        servicesMigration.AddRepositoryContextRead(config.DatabaseRead);
        IServiceProvider serviceProvider = servicesMigration.BuildServiceProvider();
        if (!serviceProvider.GetRequiredService<IHostEnvironment>().IsDevelopment())
        {
            await serviceProvider.ApplyMigrationsDbWriteAsync();
            await serviceProvider.ApplyMigrationsDbReadAsync();
        }
    }
}