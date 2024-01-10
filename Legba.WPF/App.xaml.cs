using Legba.Engine.LlmConnectors.OpenAi;
using Legba.Engine.Models;
using Legba.Engine.ViewModels;
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

        // Add Settings from User Secrets or appsettings.json
        var builder = 
            new ConfigurationBuilder()
                .AddUserSecrets<App>();

        var config = builder.Build();
        var settings = config.Get<Settings>() ?? new Settings();
        services.AddSingleton(settings);

        // Register your view model for injection
        services.AddTransient<ChatViewModel>();

        // Register your view for injection
        services.AddTransient<MainWindow>();

        // Register the HttpClientFactory, required by OpenAiConnector
        services.AddHttpClient();

        // Register the connection
        services.AddSingleton<Connection>();

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