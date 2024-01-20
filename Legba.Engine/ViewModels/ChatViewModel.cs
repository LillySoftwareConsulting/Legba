using System.Collections.ObjectModel;
using System.Windows.Input;
using Legba.Engine.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Legba.Engine.ViewModels;

public class ChatViewModel : ObservableObject
{
    #region Properties, Fields, Commands, and Events

    private readonly IServiceProvider _serviceProvider;

    public ObservableCollection<Settings.Llm> Llms { get; } = new();

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
                OnPropertyChanged(nameof(HasChatSession));
            }
        }
    }

    public bool HasChatSession => ChatSession != null;

    public ICommand SelectModelCommand { get; private set; }
    public ICommand AskCommand { get; private set; }

    #endregion

    public ChatViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var settings = _serviceProvider.GetRequiredService<Settings>();
        foreach (var llm in settings.Llms)
        {
            Llms.Add(llm);
        }

        SelectModelCommand = new TypedRelayCommand<Settings.Model>(SelectModel);
        AskCommand = new RelayCommand(async () => await ChatSession.Ask());
    }

    private void SelectModel(Settings.Model model)
    {
        var llm = Llms.First(l => l.Models.Contains(model));

        ChatSession?.Dispose();

        ChatSession = new ChatSession(_serviceProvider, llm, model);
    }
}