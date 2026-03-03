namespace AlthiraProducts.Adapters.OpenTelemetry.Ports;

public interface IOpenTelemetryService
{
    IDisposable StartActivity(string name);
    IDisposable StartInternalActivity(string name, Dictionary<string, object>? tags = null);
    void AddTag(string key, object value);
    void AddError(Exception ex, string message);
    void AddStep(string message);
}