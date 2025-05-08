using Legba.Engine.Models;
using Legba.Engine.Models.OpenAi;
using Legba.Engine.Services;
using Legba.Engine.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Legba;

public partial class App : System.Windows.Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        IServiceCollection services = new ServiceCollection();

        // Load settings from user secrets
#if DEBUG
        var builder = new ConfigurationBuilder()
            .AddUserSecrets<App>();
#else
       // Load settings from appsettings.json
       var builder = 
           new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
#endif
        var config = builder.Build();
        var settings = config.Get<Settings>() ?? new Settings();

        // Register the Settings object for injection
        services.AddSingleton(settings);

        // Register Views for injection
        services.AddTransient<MainWindow>();
        services.AddTransient<ChatSessionViewModel>();

        // Add services for injection
        services.AddSingleton<OpenAiConnector>();
        services.AddSingleton<LlmConnectorFactory>();
        services.AddSingleton(provider => new PromptRepository("Legba.db"));

        // Register transient classes for injection
        services.AddTransient<ChatSession>();

        // Registers IHttpClientFactory
        services.AddHttpClient();

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _serviceProvider.GetRequiredService<MainWindow>().Show();
    }
}
