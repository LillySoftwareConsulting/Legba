using Legba.Engine.LlmConnectors.OpenAi;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Legba.Engine.Models;

public class ChatSession : INotifyPropertyChanged, IDisposable
{
    #region Properties, Commands, and Events

    private readonly Connection _connection;

    private Persona _persona = new();
    private Purpose _purpose = new();
    private Process _process = new();
    private string _prompt = string.Empty;
    private bool _disposed = false; // To detect redundant calls

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
    public ObservableCollection<Usage> TokenUsages { get; set; } = new();

    public int GrandTotalPromptTokens { get { return TokenUsages.Sum(u => u.PromptTokens); } }
    public int GrandTotalCompletionTokens { get { return TokenUsages.Sum(u => u.CompletionTokens); } }
    public int GrandTotalTokens { get { return TokenUsages.Sum(u => u.TotalTokens); } }

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    public ChatSession(Connection connection)
    {
        _connection = connection;

        TokenUsages.CollectionChanged += OnTokenUsagesCollectionChanged;
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

        // Remove "Thinking..." message
        Messages.Remove(Messages.Last());

        AddMessage(Enums.Role.Assistant, response.Choices[0].Message?.Content);

        TokenUsages.Add(response.Usage);
    }

    private void AddMessage(Enums.Role role, string content)
    {
        Messages.Add(new Message { Role = role, Content = content });
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnTokenUsagesCollectionChanged(object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(GrandTotalPromptTokens));
        OnPropertyChanged(nameof(GrandTotalCompletionTokens));
        OnPropertyChanged(nameof(GrandTotalTokens));
    }

    #region Implementation of IDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        { 
            return; 
        }

        if (disposing)
        {
            Messages.Clear();
            TokenUsages.Clear();

            TokenUsages.CollectionChanged -= OnTokenUsagesCollectionChanged;
        }

        _disposed = true;
    }

    ~ChatSession()
    {
        Dispose(false);
    }

    #endregion
}