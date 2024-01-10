using System.ComponentModel;
using System.Windows.Input;
using Legba.Engine.LlmConnectors.OpenAi;
using Legba.Engine.Models;

namespace Legba.Engine.ViewModels;

public class ChatViewModel : INotifyPropertyChanged
{
    #region Properties, Commands, and Events

    private readonly Connection _connection;
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

    public ChatViewModel(Connection connection)
    {
        _connection = connection;

        StartNewSession();

        AskCommand = new RelayCommand(async () => await ChatSession.Ask());
        StartNewSessionCommand = new RelayCommand(StartNewSession);
    }

    private void StartNewSession()
    {
        ChatSession?.Dispose();

        ChatSession = new ChatSession(_connection);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}