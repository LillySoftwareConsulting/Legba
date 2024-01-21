using Legba.Engine.LlmConnectors;
using Legba.Engine.LlmConnectors.OpenAi;
using Legba.Engine.Models;
using Legba.Engine.Services;
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
        services.AddTransient<MainWindow>();
        services.AddTransient<PromptPrefixSelectionViewModel<Persona>>();
        services.AddTransient<PromptPrefixSelectionViewModel<Persuasion>>();
        services.AddTransient<PromptPrefixSelectionViewModel<Process>>();
        services.AddTransient<PromptPrefixSelectionViewModel<Purpose>>();
        services.AddTransient<PromptPrefixSelectionView<Persona>>();
        services.AddTransient<PromptPrefixSelectionView<Persuasion>>();
        services.AddTransient<PromptPrefixSelectionView<Process>>();
        services.AddTransient<PromptPrefixSelectionView<Purpose>>();
        services.AddTransient<PromptPrefixEditorViewModel<Persona>>();
        services.AddTransient<PromptPrefixEditorViewModel<Persuasion>>();
        services.AddTransient<PromptPrefixEditorViewModel<Process>>();
        services.AddTransient<PromptPrefixEditorViewModel<Purpose>>();
        services.AddTransient<PromptPrefixEditorView<Persona>>();
        services.AddTransient<PromptPrefixEditorView<Persuasion>>();
        services.AddTransient<PromptPrefixEditorView<Process>>();
        services.AddTransient<PromptPrefixEditorView<Purpose>>();

        // Register 'service' objects for injection
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

        // Resolve the main window and set its DataContext to the injected view model
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        var viewModel = _serviceProvider.GetRequiredService<ChatViewModel>();

        mainWindow.DataContext = viewModel;
        mainWindow.Show();
    }
}