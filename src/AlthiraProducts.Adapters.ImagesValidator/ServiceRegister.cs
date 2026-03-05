using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AlthiraProducts.Adapters.ImagesValidator;

public static class ServiceRegister
{
    public static void AddImageValidator(
        this IServiceCollection serviceCollection,
        string assemblyImageValidator)
    {
        Assembly assembly = Assembly.Load(assemblyImageValidator);
        serviceCollection.Scan(scan =>
            scan.FromAssemblies(assembly)
            .AddClasses(clases =>
                clases.AssignableToAny(typeof(ITransient))
            )
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );
    }
}