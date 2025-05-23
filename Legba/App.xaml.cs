﻿using Legba.Engine.Models;
using Legba.Engine.Models.OpenAi;
using Legba.Engine.Services;
using Legba.Engine.ViewModels;
using Legba.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace Legba;

public partial class App : System.Windows.Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        // Setup the application data directory for the app
        string appDataPath = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Legba");
        Directory.CreateDirectory(appDataPath);

        // Setup the services for the app
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
        services.AddSingleton(provider => new PromptRepository(Path.Combine(appDataPath, "Legba.db")));

        // Register transient classes for injection
        services.AddTransient<ChatSession>();
        services.AddTransient<PromptPrefixSelectionView<Personality>>();
        services.AddTransient<PromptPrefixSelectionViewModel<Personality>>();
        services.AddTransient<PromptPrefixEditorView<Personality>>();
        services.AddTransient<PromptPrefixEditorViewModel<Personality>>();

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
