using Legba.Engine.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Legba.Engine.ViewModels;

public class ChatSessionViewModel : ObservableObject
{
    #region Private fields

    private readonly IServiceProvider _serviceProvider;

    private ChatSession? _chatSession;

    #endregion

    #region Properties and Commands

    public ObservableCollection<Settings.Llm> Llms { get; } = [];

    public ChatSession? ChatSession
    {
        get => _chatSession;
        set
        {
            // Early exit, if object was not changed
            if (_chatSession == value)
            {
                return;
            }

            // Change the value, with proper unsubscribe, dispose, subscribe,
            // and property changed notifications.
            if (_chatSession != null)
            {
                _chatSession.Messages.CollectionChanged -= Messages_CollectionChanged;
                _chatSession.Dispose();
            }

            _chatSession = value;

            OnPropertyChanged(nameof(ChatSession));
            OnPropertyChanged(nameof(HasChatSession));
            OnPropertyChanged(nameof(HasChatMessages));

            if (_chatSession != null)
            {
                _chatSession.Messages.CollectionChanged += Messages_CollectionChanged;
            }
        }
    }

    public bool HasChatSession => ChatSession != null;
    public bool HasChatMessages => ChatSession?.Messages.Count > 0;

    public ICommand SelectModelCommand { get; private set; }
    public ICommand AskCommand { get; private set; }
    public ICommand RemoveSourceCodeFileCommand { get; }

    #endregion

    public ChatSessionViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var settings = _serviceProvider.GetRequiredService<Settings>();

        foreach (var llm in settings.Llms)
        {
            Llms.Add(llm);
        }

        SelectModelCommand = new TypedRelayCommand<Settings.Model>(SelectModel);
        AskCommand = new RelayCommand(async () => await ChatSession.AskAsync());
        RemoveSourceCodeFileCommand = new TypedRelayCommand<object>(RemoveSourceCodeFile);
    }

    private void Messages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasChatMessages));
    }

    private void SelectModel(Settings.Model model)
    {
        ChatSession = new ChatSession(_serviceProvider, Llms.LlmWithModel(model), model);
    }

    private void RemoveSourceCodeFile(object file)
    {
        if (ChatSession == null)
        {
            return;
        }

        if (file is string path)
        {
            ChatSession.SourceCodeFiles.Remove(path);
        }
    }
}
