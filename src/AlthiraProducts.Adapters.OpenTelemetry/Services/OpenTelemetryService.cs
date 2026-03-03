using AlthiraProducts.Adapters.OpenTelemetry.Ports;
using System.Diagnostics;

namespace AlthiraProducts.Adapters.OpenTelemetry.Services;


public class OpenTelemetryService(ActivitySource activitySource) : IOpenTelemetryService
{
    public IDisposable StartActivity(string name) => activitySource.StartActivity(name)!;

    public void AddTag(string key, object value) => Activity.Current?.SetTag(key, value);

    public IDisposable StartInternalActivity(string name, Dictionary<string, object>? tags = null)
    {
        Activity activity = activitySource.StartActivity(name)!;
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

    public void AddStep( string message)
    {
        Activity.Current?.AddEvent(new(message));
    }
}