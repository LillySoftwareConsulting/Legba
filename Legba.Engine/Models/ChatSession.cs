using OpenAiConnector;
using OpenAiConnector.Models;
using OpenAiConnector.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Legba.Engine.Models;

public class ChatSession : INotifyPropertyChanged
{
    #region Properties, Commands, and Events

    private readonly Connection _connection;

    private Persona _persona = new();
    private Purpose _purpose = new();
    private Process _process = new();
    private string _prompt = string.Empty;

    public bool IncludePriorMessages { get; set; } = true;

    public Persona Persona 
    { 
        get => _persona;
        set 
        { 
            _persona = value;
            OnPropertyChanged(nameof(Persona));
        } 
    }

    public Purpose Purpose 
    { 
        get => _purpose;
        set 
        { 
            _purpose = value; 
            OnPropertyChanged(nameof(Purpose));
        }
    }

    public Process Process
    {
        get => _process;
        set
        {
            _process = value;
            OnPropertyChanged(nameof(Process));
        }
    }

    public string Prompt
    {
        get => _prompt;
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

    public int GrandTotalPromptTokens { get { return Usages.Sum(u => u.PromptTokens); } }
    public int GrandTotalCompletionTokens { get { return Usages.Sum(u => u.CompletionTokens); } }
    public int GrandTotalTokens { get { return Usages.Sum(u => u.TotalTokens); } }

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    public ChatSession(Connection connection)
    {
        _connection = connection;

        Usages.CollectionChanged += OnUsagesCollectionChanged;
    }

    public async Task Ask()
    {
        // Store prompt in a variable so we can clear it before the response is received.
        // This prevents the user from spamming the ask button.
        var prompt = Prompt;
        Prompt = string.Empty;

        // TODO: Record selected Persona, Purpose, and Process with prompt, for history.
        AddMessage(Enums.Role.User, prompt);

        AddMessage(Enums.Role.Assistant, "Thinking...");

        var completePrompt = 
            $"{Persona.Description}\n{Purpose.Description}\n{Process.Description}\n{prompt}";

        var response =
            await _connection.CallOpenAiApiAsync(completePrompt,
            IncludePriorMessages ? Messages.ToList() : null);

        Messages.Remove(Messages.Last());

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