using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using System.Net;

namespace AlthiraProducts.Adapters.WebApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger; // Tipo explícito
    private readonly IOpenTelemetryService _telemetryService;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IOpenTelemetryService telemetryService)
    {
        _next = next;
        _logger = logger;
        _telemetryService = telemetryService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Request.Body.Position = 0;
        using StreamReader reader = new (context.Request.Body, leaveOpen: true);
        string requestBody = await reader.ReadToEndAsync();


        _telemetryService.AddTag("http.status_code", "500");
        _telemetryService.AddTag("http.request.body", requestBody);
        _telemetryService.AddError(exception, "Unhandled Exception in API Pipeline");

        _logger.LogError(exception, "An unhandled exception has occurred while executing the request.");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        string result = System.Text.Json.JsonSerializer.Serialize(new
        {
            error = "Internal Server Error",
            message = exception.Message
        });

        await context.Response.WriteAsync(result);
    }
}
