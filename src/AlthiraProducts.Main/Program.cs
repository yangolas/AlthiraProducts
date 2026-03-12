using AlthiraProducts.Main.Process;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Globalization;


var cultureInfo = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

IConfigurationRoot configurationRoot = builder.Build();
IConfigurationSection configurationSection = configurationRoot.GetSection("Config");
AlthiraProductsSettings config = configurationSection.Get<AlthiraProductsSettings>()!;

string startProcess = args.Length > 0 ? args[0] : config.StartProcess.DebugAllProcess;

Task runTask = startProcess switch
{
    string @case when @case.Equals(config.StartProcess.WebApi, StringComparison.OrdinalIgnoreCase)
        => WebApiProcess.Start(configurationSection, config),

    string @case when @case.Equals(config.StartProcess.Consumer, StringComparison.OrdinalIgnoreCase)
        => ConsumerProcess.Start(configurationSection, config),

    string @case when @case.Equals(config.StartProcess.Outbox, StringComparison.OrdinalIgnoreCase)
        => OutboxProcess.Start(configurationSection, config),

    string @case when @case.Equals(config.StartProcess.Image, StringComparison.OrdinalIgnoreCase)
        => ImageProcess.Start(configurationSection, config),
   
    string @case when @case.Equals(config.StartProcess.Migration, StringComparison.OrdinalIgnoreCase)
        => DbMigrationProcess.Start(config),

    _ => Task.WhenAll(
            WebApiProcess.Start(configurationSection, config),
            ConsumerProcess.Start(configurationSection, config),
            OutboxProcess.Start(configurationSection, config),
            ImageProcess.Start(configurationSection, config)
         )
};

await runTask;