using OpenAiConnector;
using OpenAiConnector.Models;
using OpenAiConnector.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Specialized;

namespace Legba.WPF;

public class ChatViewModel : INotifyPropertyChanged
{
    private readonly Connection _connection;
    private string _prompt = string.Empty;

    public ICommand StartNewSessionCommand { get; }
    public ICommand AskCommand { get; }

    public string Prompt
    {
        get { return _prompt; }
        set
        {
            if (_prompt != value)
            {
                _prompt = value;
                OnPropertyChanged(nameof(Prompt));
            }
        }
    }

    public ObservableCollection<Message> Messages { get; set; } = new();
    public ObservableCollection<Usage> Usages { get; set; } = new();

    public bool IncludePriorMessages { get; set; } = true;

    public event PropertyChangedEventHandler? PropertyChanged;

    public int GrandTotalPromptTokens { get { return Usages.Sum(u => u.PromptTokens); } }
    public int GrandTotalCompletionTokens { get { return Usages.Sum(u => u.CompletionTokens); } }
    public int GrandTotalTokens { get { return Usages.Sum(u => u.TotalTokens); } }

    public ChatViewModel(Connection connection)
    {
        _connection = connection;

        Prompt = "What time and temperature should you use to cook frozen onion rings in a convection oven?";
        AskCommand = new RelayCommand(async () => await Ask());
        StartNewSessionCommand = new RelayCommand(StartNewSession);

        Usages.CollectionChanged += OnUsagesCollectionChanged;
    }

    private void StartNewSession()
    {
        Prompt = string.Empty;
        Messages.Clear();
        Usages.Clear();
    }

    private async Task Ask()
    {
        // Store prompt in a variable so we can clear it before the response is received.
        // This prevents the user from spamming the ask button.
        var prompt = Prompt;
        Prompt = string.Empty;

        AddMessage(Enums.Role.User, prompt);

        var response =
            await _connection.CallOpenAiApiAsync(prompt,
            IncludePriorMessages ? Messages.ToList() : null);

        AddMessage(Enums.Role.Assistant, response.Choices[0].Message?.Content);

        Usages.Add(response.Usage);
    }

    private void AddMessage(Enums.Role role, string content)
    {
        Messages.Add(new Message { Role = role, Content = content });
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnUsagesCollectionChanged(object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(GrandTotalPromptTokens));
        OnPropertyChanged(nameof(GrandTotalCompletionTokens));
        OnPropertyChanged(nameof(GrandTotalTokens));
    }
}