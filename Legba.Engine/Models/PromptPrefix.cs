using LiteDB;
using System.Text.Json.Serialization;

namespace Legba.Engine.Models;

public abstract class PromptPrefix : ObservableObject
{
    private string _name = string.Empty;
    private string _text = string.Empty;

    // Default to Guid.Empty so that we can use this as a sentinel value
    // to indicate that the PromptPrefix has not been saved yet.
    public Guid Id { get; set; } = Guid.Empty;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(CanBeSaved));
        }
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged(nameof(Text));
            OnPropertyChanged(nameof(CanBeSaved));
        }
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool CanBeSaved => 
        !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Text);
}