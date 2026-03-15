namespace AlthiraProducts.BuildingBlocks.Application.Settings;

public class IngressLocalKubernetesSettings
{
    public int Port { get; set; }
    public string Host { get; set; } = null!;
}