using AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
using System.Text;

namespace AlthiraProducts.Adapters.OpenTelemetry.Services;


public class OpenTelemetryService(ActivitySource _activitySource) : IOpenTelemetryService
{
    private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;
    public IDisposable StartActivity(string name) => _activitySource.StartActivity(name)!;

    public void AddTag(string key, object value) => Activity.Current?.SetTag(key, value);

    public IDisposable StartInternalActivity(string name, Dictionary<string, object>? tags = null)
    {
        Activity activity = _activitySource.StartActivity(name)!;
        if (tags != null && activity != null)
        {
            foreach (var tag in tags) activity.SetTag(tag.Key, tag.Value);
        }
        return activity!;
    }

    public void AddError(Exception ex, string message)
    {
        Activity.Current?.SetStatus(ActivityStatusCode.Error, message);
        Activity.Current?.AddException(ex);
    }

    public void AddError(string message)
    {
        Activity.Current?.SetStatus(ActivityStatusCode.Error, message);
    }

    public void AddStep( string message)
    {
        Activity.Current?.AddEvent(new(message));
    }

    public void InjectContext(IDictionary<string, object?> headers)
    {
        var contextToInject = Activity.Current?.Context ?? default;
        Propagator.Inject(new PropagationContext(contextToInject, Baggage.Current), headers, (dict, key, value) =>
        {
            dict[key] = value;
        });
    }

    public void ExtractContext(IDictionary<string, object?> headers)
    {
        var parentContext = Propagator.Extract(default, headers, (dict, key) =>
        {
            if (dict.TryGetValue(key, out var value) && value != null)
            {
                var stringValue = value is byte[] bytes ? Encoding.UTF8.GetString(bytes) : value.ToString();
                return new[] { stringValue ?? string.Empty };
            }
            return Enumerable.Empty<string>();
        });
        _activitySource.StartActivity("RabbitMQ Consumer", ActivityKind.Consumer, parentContext.ActivityContext);
    }
}