using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Extensions.DependencyInjection;
using Legba.Engine.Models.OpenAi;
using Legba.Engine.Services;
using CSharpExtender.ExtensionMethods;

namespace Legba.Engine.Models;

public class ChatSession : ObservableObject, IDisposable
{
    #region Private fields

    private readonly OpenAiConnector _llmConnector;

    private bool _disposed = false; // To detect redundant calls

    private string _prompt = string.Empty;
    private Personality _personality = new();
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

    public Personality Personality
    {
        get => _personality;
        set
        {
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

    public ObservableCollection<TokenSummary> TokenSummaries { get; set; } = [];

    public string ModelName => $"{_llmConnector.Llm.Name} | {_llmConnector.Model.Name}";

    public bool HasMessages => Messages.Count > 0;

    public int GrandTotalRequestTokenCount => 
        TokenSummaries.Sum(u => u.RequestTokenCount);
    public int GrandTotalResponseTokenCount => 
        TokenSummaries.Sum(u => u.ResponseTokenCount);
    public int GrandTotalTokenCount => 
        TokenSummaries.Sum(u => u.TotalTokenCount);

    #endregion

    public ChatSession(IServiceProvider serviceProvider, 
        Settings.Llm llm, Settings.Model model)
    {
        var factory = serviceProvider.GetRequiredService<LlmConnectorFactory>();
        _llmConnector = factory.GetLlmConnector(llm, model);

        Messages.CollectionChanged += OnMessagesCollectionChanged;
        TokenSummaries.CollectionChanged += OnTokenUsagesCollectionChanged;
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
        // On first request submission, include the personality and source code (if any)
        if (Messages.None())
        {
            if (Personality.Text.IsNotNullEmptyOrWhitespace())
            {
                AddMessage(Enums.Role.System, Personality.Text);
            }

            if (SourceCode.IsNotNullEmptyOrWhitespace())
            {
                AddMessage(Enums.Role.User, SourceCode, true);
            }
        }

        // Add prompt to messages
        AddMessage(Enums.Role.User, Prompt);

        // Clear prompt in UI
        Prompt = string.Empty;

        var llmRequest = BuildLlmRequest();

        AddMessage(Enums.Role.Assistant, "Thinking...");

        var response = await _llmConnector.AskAsync(llmRequest);

        // Remove "Thinking..." message
        Messages.Remove(Messages.Last());

        AddMessage(Enums.Role.Assistant, response.Text);

        TokenSummaries.Add(new TokenSummary
        {
            RequestTokenCount = response.RequestTokenCount,
            ResponseTokenCount = response.ResponseTokenCount
        });
    }

    #region Private supporting methods

    private void AddMessage(Enums.Role role, string content, bool isInitialSourceCode = false)
    {
        var message = new Message
        { 
            Role = role, 
            Content = content, 
            IsInitialSourceCode = isInitialSourceCode
        };

        Messages.Add(message);
    }

    private LegbaRequest BuildLlmRequest()
    {
        return new LegbaRequest
        {
            Messages = Messages.ToList(),
            Temperature = 0.5f
        };
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
            TokenSummaries.Clear();

            TokenSummaries.CollectionChanged -= OnTokenUsagesCollectionChanged;
        }

        _disposed = true;
    }

    ~ChatSession()
    {
        Dispose(false);
    }

    #endregion
}