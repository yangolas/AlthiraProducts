using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlthiraProducts.BuildingBlocks.Application.Behaviors;

public class TracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly IOpenTelemetryService _telemetryService;
    private readonly ILogger<TracingBehavior<TRequest, TResponse>> _logger;

    public TracingBehavior(IOpenTelemetryService telemetryService, ILogger<TracingBehavior<TRequest, TResponse>> logger)
    {
        _telemetryService = telemetryService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        _telemetryService.AddStep($"MediatR: {requestName}");
        _telemetryService.AddTag("mediatr.request", requestName);


        try
        {
            TResponse response = await next();
            _telemetryService.AddStep($"MediatR: Success {requestName}");
            _telemetryService.AddTag("mediatr.handler.state", "completed");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in MediatR Pipeline for {RequestName}", requestName);
            _telemetryService.AddError(ex, $"Error in MediatR Pipeline for {requestName}");
            throw;
        }
    }
}
