namespace AlthiraProducts.BuildingBlocks.Application.Ports.OpenTelemetry;

public interface IOpenTelemetryService
{
    IDisposable StartActivity(string name);
    IDisposable StartInternalActivity(string name, Dictionary<string, object>? tags = null);
    void AddTag(string key, object value);
    void AddError(Exception ex, string message);
    void AddError(string message);
    void AddStep(string message);
    Dictionary<string, object?> InjectContextGetDictionary();
    public string InjectContextGetString();
    void ExtractContext(IDictionary<string, object?> headers);
}