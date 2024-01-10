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

        // Add Settings from User Secrets
        var builder = new ConfigurationBuilder()
            .AddUserSecrets<App>();

        var config = builder.Build();
        var settings = config.Get<Settings>() ?? new Settings();

        // Register the Settings object for injection
        services.AddSingleton(settings);

        // Register view and viewmodel objects for injection
        services.AddTransient<ChatViewModel>();
        services.AddTransient<MainWindow>();

        // Register 'service' objects for injection
        services.AddSingleton<Connection>();

        // Register the HttpClientFactory, for calls to external LLMs
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