using System.Collections.ObjectModel;
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

    private Settings.Llm? _llm;
    private Settings.Model? _model;
    public ObservableCollection<Settings.Llm> Llms { get; } = new();
    public ICommand SelectModelCommand { get; private set; }

    public ChatViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var settings = _serviceProvider.GetRequiredService<Settings>();
        foreach (var llm in settings.Llms)
        {
            Llms.Add(llm);
        }

        // TODO: Handle from user input
        _llm = settings.Llms.FirstOrDefault();
        _model = _llm?.Models.First(m => m.Name.Equals("GPT-4 (preview)"));

        StartNewSession();

        // These only need to be created once for the ViewModel, not for each ChatSession.
        AskCommand = new RelayCommand(async () => await ChatSession.Ask());
        StartNewSessionCommand = new RelayCommand(StartNewSession);
        SelectModelCommand = new GenericRelayCommand<Settings.Model>(SelectModel);
    }

    private void SelectModel(Settings.Model model)
    {
        ;
        var llm = Llms.FirstOrDefault(l => l.Models.Contains(model));
    }

    private void StartNewSession()
    {
        ChatSession?.Dispose();

        ChatSession = new ChatSession(_serviceProvider, _llm!, _model!);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}