namespace AlthiraProducts.Main.Settings.Models;

public class MessageBrokerSettings
{
    public string HostName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Port { get; set; }
    public int ConsumerConcurrency { get; set; } 
}