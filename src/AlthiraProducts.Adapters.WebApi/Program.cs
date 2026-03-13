
using System.Diagnostics;

namespace AlthiraProducts.Adapters.WebApi;

public class WebApi
{
    private readonly WebApplicationBuilder _builder;

    public WebApi()
    {
        string baseDiretory = AppDomain.CurrentDomain.BaseDirectory;
        Directory.SetCurrentDirectory(baseDiretory);
        _builder = WebApplication.CreateBuilder(
            new WebApplicationOptions()
            {
                ApplicationName = "AlthiraProducts.Adapters.WebApi",
                EnvironmentName = "Development"
            }
        );
    }

    public void Configure(IServiceCollection services , CorsSettings corsSettings)
    {
        _builder.Services.AddControllers();
        _builder.Services.AddEndpointsApiExplorer();
        _builder.Services.AddSwaggerGen();
        _builder.Services.AddCors(options =>
        {
            options.AddPolicy("AlthiraCors", policy =>
            {
                policy.WithOrigins(corsSettings.AllowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
        foreach (var service in services)
        {
            _builder.Services.Add(service);
        }
    }

    public Task Start()
    {
        var app = _builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
            bool isContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
            if (isWindows && !isContainer)
            {
                var url = "http://localhost:5000/swagger/index.html";
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
        }
        app.UseCors("AlthiraCors");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app.RunAsync();
    }
}