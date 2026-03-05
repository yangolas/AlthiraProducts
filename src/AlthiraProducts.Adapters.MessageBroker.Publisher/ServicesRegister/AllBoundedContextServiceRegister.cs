using AlthiraProducts.Adapters.MessageBroker.Publisher.Services.Extenisons;
using AlthiraProducts.BuildingBlocks.Application.EventModel;
using AlthiraProducts.BuildingBlocks.Application.Ports.MessageBrokerPublisher;
using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using AlthiraProducts.BuildingBlocks.Application.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.Adapters.MessageBroker.Publisher.ServicesRegister;

internal static class AllBoundedContextServiceRegister
{
    internal static void AddAllBoundedContextServices(
        this IServiceCollection services,
        MessageBrokerSettings messageBrokerSettings,
        EventSettings eventSettings)
    {
        services.AddScoped<IPublisherService<Event>, ProductPublisherService>(provider =>
        {
            ILogger<ProductPublisherService> logger = provider.GetRequiredService<ILogger<ProductPublisherService>>();
            IOpenTelemetryService openTelemetryService = provider.GetRequiredService<IOpenTelemetryService>();
            return new ProductPublisherService(logger, openTelemetryService, messageBrokerSettings, eventSettings.ProductEventChannels);
        });
    }
}