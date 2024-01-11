using Legba.Engine.LlmConnectors.OpenAi;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Legba.Engine.Models;

public class ChatSession : INotifyPropertyChanged, IDisposable
{
    #region Properties, Fields, Commands, and Events

    private readonly OpenAiConnector _connection;

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

    public ObservableCollection<Message> Messages { get; set; } = [];
    public ObservableCollection<Usage> TokenUsages { get; set; } = [];

    public int GrandTotalPromptTokens 
        => TokenUsages.Sum(u => u.PromptTokens);
    public int GrandTotalCompletionTokens 
        => TokenUsages.Sum(u => u.CompletionTokens);
    public int GrandTotalTokens 
        => TokenUsages.Sum(u => u.TotalTokens);

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    public ChatSession(OpenAiConnector connector)
    {
        _connection = connector;

        TokenUsages.CollectionChanged += OnTokenUsagesCollectionChanged;
    }

    public async Task Ask()
    {
        // Store prompt in a variable so we can clear it before the response is received.
        // This prevents the user from spamming the ask button,
        // due to the button not being enabled when the Prompt property is empty.
        var completePrompt = BuildPrompt();
        Prompt = string.Empty;

        // TODO: Record selected Persona, Purpose, and Process with prompt, for history.
        AddMessage(Enums.Role.User, completePrompt);

        var llmRequest = BuildLlmRequest();

        AddMessage(Enums.Role.Assistant, "Thinking...");

        var response = await _connection.AskAsync(llmRequest);

        // Remove "Thinking..." message
        Messages.Remove(Messages.Last());

        AddMessage(Enums.Role.Assistant, response.Text);

        //TokenUsages.Add(response.Usage);
    }

    #region Private supporting methods

    private void OnTokenUsagesCollectionChanged(object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(GrandTotalPromptTokens));
        OnPropertyChanged(nameof(GrandTotalCompletionTokens));
        OnPropertyChanged(nameof(GrandTotalTokens));
    }

    private void AddMessage(Enums.Role role, string content)
    {
        Messages.Add(new Message { Role = role, Content = content });
    }

    private LegbaRequest BuildLlmRequest()
    {
        return new LegbaRequest
        {
            Messages = IncludePriorMessages ? Messages.ToList() : [],
            Temperature = 0.5f
        };
    }

    private string BuildPrompt()
    {
        var sb = new StringBuilder();

        if(Persona.Description.IsNotNullEmptyOrWhitespace())
        {
            sb.AppendLine(Persona.Description);
        }
        if(Purpose.Description.IsNotNullEmptyOrWhitespace())
        {
            sb.AppendLine(Purpose.Description);
        }
        if (Process.Description.IsNotNullEmptyOrWhitespace())
        {
            sb.AppendLine(Process.Description);
        }

        sb.Append(Prompt);

        return sb.ToString();
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

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