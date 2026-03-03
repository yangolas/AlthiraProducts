using AlthiraProducts.Adapters.Repository.Read.Context;
using AlthiraProducts.Adapters.Repository.Read.Ports;
using AlthiraProducts.Main.Settings.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AlthiraProducts.Adapters.Repository.Read.Services;

public static class ServiceRegister
{
    public static void AddRepositoryContextRead(
        this IServiceCollection serviceCollection,
        DatabaseReadSettings databseReadSettings)
    {
        serviceCollection.AddSingleton(new ProductReadContext(databseReadSettings));
    }

    public static void AddRepositoryRead(
        this IServiceCollection serviceCollection,
        string assemblyRepositoryRead)
    {
        Assembly assembly = Assembly.Load(assemblyRepositoryRead);
        serviceCollection.Scan(scan =>
            scan.FromAssemblies(assembly)
            .AddClasses(clases =>
                clases.AssignableToAny(typeof(ITransientRepositoryRead))
            )
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );
    }
}