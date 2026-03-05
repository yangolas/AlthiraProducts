using AlthiraProducts.BuildingBlocks.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AlthiraProducts.Products.Application;

public static class ServiceRegistration
{
    public static void AddApplicationProduct(this IServiceCollection services, string aplicationUserAssembly)
    {
        var assembly = Assembly.Load(aplicationUserAssembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TracingBehavior<,>));
        });
    }
}