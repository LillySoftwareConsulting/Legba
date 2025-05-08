using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Legba.Engine.Models.OpenAi;
using Legba.Engine.Services;

namespace Legba.Engine.Models;

public class ChatSession : ObservableObject, IDisposable
{
    #region Private fields

    private readonly OpenAiConnector _llmConnector;

    private bool _disposed = false; // To detect redundant calls

    private string _prompt = string.Empty;
    private string _personality = string.Empty;
    private string _sourceCode = string.Empty;

    #endregion

    #region Properties

    public string Prompt
    {
        get => _prompt;
        set
        {
            if (_prompt == value)
            {
                return;
            }

            _prompt = value;

            OnPropertyChanged(nameof(Prompt));
        }
    }

    public string Personality
    {
        get => _personality;
        set
        {
            if (_personality == value)
            {
                return;
            }

            _personality = value;

            OnPropertyChanged(nameof(Personality));
        }
    }

    public string SourceCode
    {
        get => _sourceCode;
        set
        {
            if (_sourceCode == value)
            {
                return;
            }

            _sourceCode = value;

            OnPropertyChanged(nameof(SourceCode));
        }
    }

    public ObservableCollection<Message> Messages { get; set; } = [];

    public ObservableCollection<TokenSummary> TokenUsages { get; set; } = [];

    public string ModelName => $"{_llmConnector.Llm.Name} | {_llmConnector.Model.Name}";

    public bool HasMessages => Messages.Count > 0;

    public int GrandTotalRequestTokenCount => 
        TokenUsages.Sum(u => u.RequestTokenCount);
    public int GrandTotalResponseTokenCount => 
        TokenUsages.Sum(u => u.ResponseTokenCount);
    public int GrandTotalTokenCount => 
        TokenUsages.Sum(u => u.TotalTokenCount);

    #endregion

    public ChatSession(IServiceProvider serviceProvider, 
        Settings.Llm llm, Settings.Model model)
    {
        var factory = serviceProvider.GetRequiredService<LlmConnectorFactory>();
        _llmConnector = factory.GetLlmConnector(llm, model);

        Messages.CollectionChanged += OnMessagesCollectionChanged;
        TokenUsages.CollectionChanged += OnTokenUsagesCollectionChanged;
    }

    #region Eventhandlers

    private void OnMessagesCollectionChanged(object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasMessages));
    }

    private void OnTokenUsagesCollectionChanged(object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(GrandTotalRequestTokenCount));
        OnPropertyChanged(nameof(GrandTotalResponseTokenCount));
        OnPropertyChanged(nameof(GrandTotalTokenCount));
    }

    #endregion

    public async Task AskAsync()
    {
        // Store prompt in a variable so we can clear it before the response is received.
        // This prevents the user from spamming the ask button,
        // due to the button not being enabled when the Prompt property is empty.
        var completePrompt = BuildPrompt();

        Prompt = string.Empty;

        AddMessage(Enums.Role.User, completePrompt);

        var llmRequest = BuildLlmRequest();

        AddMessage(Enums.Role.Assistant, "Thinking...");

        var response = await _llmConnector.AskAsync(llmRequest);

        // Remove "Thinking..." message
        Messages.Remove(Messages.Last());

        AddMessage(Enums.Role.Assistant, response.Text);

        TokenUsages.Add(new TokenSummary
        {
            RequestTokenCount = response.RequestTokenCount,
            ResponseTokenCount = response.ResponseTokenCount
        });
    }

    #region Private supporting methods

    private void AddMessage(Enums.Role role, string content)
    {
        Messages.Add(new Message { Role = role, Content = content });
    }

    private LegbaRequest BuildLlmRequest()
    {
        return new LegbaRequest
        {
            Messages = Messages.ToList(),
            Temperature = 0.5f
        };
    }

    private string BuildPrompt()
    {
        var sb = new StringBuilder();

        // Only include the personality and source code if there are no messages yet,
        // since the app always includes the prior messages in the request.
        if(Messages.Count == 0)
        {
            sb.AppendLineIfNotEmpty(Personality);
            sb.AppendLineIfNotEmpty(SourceCode);
        }

        sb.Append(Prompt);

        return sb.ToString();
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