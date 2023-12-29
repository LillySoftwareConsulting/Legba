using System.Text.Json.Serialization;

namespace OpenAiConnector.Models;

public class Message
{
    [JsonPropertyName("role")]
    public Enums.Role Role { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
    [JsonIgnore]
    public bool IsSentByUser { get { return Role == Enums.Role.User; } }
}