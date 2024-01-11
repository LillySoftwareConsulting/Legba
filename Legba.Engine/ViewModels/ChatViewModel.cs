using System.ComponentModel;
using System.Windows.Input;
using Legba.Engine.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Legba.Engine.ViewModels;

public class ChatViewModel : INotifyPropertyChanged
{
    #region Properties, Fields, Commands, and Events

    private readonly IServiceProvider _serviceProvider;

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

    public ChatViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        StartNewSession();

        // These only need to be created once for the ViewModel, not for each ChatSession.
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