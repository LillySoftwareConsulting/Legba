using System.ComponentModel;
using System.Windows.Input;
using Legba.Engine.LlmConnectors;
using Legba.Engine.LlmConnectors.OpenAi;
using Legba.Engine.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Legba.Engine.ViewModels;

public class ChatViewModel : INotifyPropertyChanged
{
    private readonly IServiceProvider _serviceProvider;
    #region Properties, Commands, and Events

    private readonly ILlmConnector _connection;
    private ChatSession _chatSession;

    public ChatSession ChatSession
    {
        get { return _chatSession; }
        set
        {
            if (_chatSession != value)
            {
                _chatSession = value;
                OnPropertyChanged(nameof(ChatSession));
            }
        }
    }

    public ICommand StartNewSessionCommand { get; }
    public ICommand AskCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    public ChatViewModel(IServiceProvider serviceProvider,
        LlmConnectorFactory factory, OpenAiConnector connection)
    {
        _serviceProvider = serviceProvider;
        _connection = connection;

        _connection = factory.GetLlmConnector();

        StartNewSession();

        AskCommand = new RelayCommand(async () => await ChatSession.Ask());
        StartNewSessionCommand = new RelayCommand(StartNewSession);
    }

    private void StartNewSession()
    {
        ChatSession?.Dispose();

        ChatSession = _serviceProvider.GetRequiredService<ChatSession>();
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}