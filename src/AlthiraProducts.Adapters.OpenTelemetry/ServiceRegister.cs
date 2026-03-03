using AlthiraProducts.Adapters.OpenTelemetry.Ports;
using AlthiraProducts.Adapters.OpenTelemetry.Services;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace AlthiraProducts.Adapters.OpenTelemetry;

public static class ServiceRegister
{
    public static void AddOpenTelemetry(
        this IServiceCollection serviceCollection, 
        string serviceName,
        string connectionstring)
    {
        serviceCollection.AddSingleton(new ActivitySource(serviceName));

        serviceCollection.AddOpenTelemetry()
        .ConfigureResource(resource => resource
            .AddService(serviceName))
        .WithTracing(tracing => tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation()
            .AddOtlpExporter(opt => {
                opt.Endpoint = new Uri(connectionstring);
            })
        );

        serviceCollection.AddSingleton<IOpenTelemetryService, OpenTelemetryService>();
    }
}