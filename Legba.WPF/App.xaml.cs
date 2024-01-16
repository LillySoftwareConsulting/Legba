using Legba.Engine.LlmConnectors;
using Legba.Engine.LlmConnectors.OpenAi;
using Legba.Engine.Models;
using Legba.Engine.ViewModels;
using Legba.WPF.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Legba.WPF;

public partial class App : Application
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

        // Register view and viewmodel objects for injection
        services.AddTransient<ChatViewModel>();
        services.AddTransient<PromptPrefixSelectionViewModel>();
        services.AddTransient<MainWindow>();
        services.AddTransient<PromptPrefixSelection>();

        // Register 'service' objects for injection
        services.AddSingleton<OpenAiConnector>();
        services.AddSingleton<LlmConnectorFactory>();

        // Register transient classes for injection
        services.AddTransient<ChatSession>();

        // Registers IHttpClientFactory
        services.AddHttpClient();

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Resolve the main window and set its DataContext to the injected view model
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        var viewModel = _serviceProvider.GetRequiredService<ChatViewModel>();

        mainWindow.DataContext = viewModel;
        mainWindow.Show();
    }
}