namespace AlthiraProducts.Main.Settings.Models;

public class StartProcessSettings 
{
    public string DebugAllProcess { get; set; } = null!;
    public string WebApi { get; set; } = null!;
    public string Outbox { get; set; } = null!;
    public string Image { get; set; } = null!;
    public string Consumer { get; set; } = null!;
    public string Migration { get; set; } = null!;
}