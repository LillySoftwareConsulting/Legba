using KeyReader;
using Legba.Engine.ViewModels;
using LlmConnectors.OpenAi.Services;
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

        // Register your view model for injection
        services.AddTransient<ChatViewModel>();

        // Register your view for injection
        services.AddTransient<MainWindow>();

        // Register appropriate key reader(s) for injection
        // The reader types for the API key and organization ID can be different types.
        // For example, get the API key from an environment variable and
        // get the organization ID from a JSON file.
        //
        // Only using separate functions in this demonstation code,
        // to make this simpler to read.
        //AddEnvironmentVariableKeyReader(services);
        //AddJsonFileKeyReader(services);
        AddUserSecretsKeyReader(services);

        // Register the HttpClientFactory, required by OpenAiConnector
        services.AddHttpClient();

        // Register the connection
        services.AddSingleton<Connection>();

        _serviceProvider = services.BuildServiceProvider();
    }

    #region KeyReader setup samples

    // EnvironmentVariableKeyReader sample
    private static void AddEnvironmentVariableKeyReader(IServiceCollection services)
    {
        services.AddSingleton<IApiKeyReader, EnvironmentVariableKeyReader>();
    }

    // JsonFileKeyReader sample
    private static void AddJsonFileKeyReader(IServiceCollection services)
    {
        services.AddSingleton<IApiKeyReader>(provider =>
            new JsonFileKeyReader("appsettings.json", "keys:openAi_ApiKey"));
    }

    // UserSecretKeyReader sample
    private static void AddUserSecretsKeyReader(IServiceCollection services)
    {
        var builder = new ConfigurationBuilder().AddUserSecrets<App>();
        var config = builder.Build();

        services.AddSingleton<IApiKeyReader>(provider =>
            new UserSecretsKeyReader(config, "keys:openAi_ApiKey"));
    }

    #endregion

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