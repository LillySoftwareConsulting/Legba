using System.Text.Json.Serialization;

namespace OpenAiConnector.Models;

public class Request
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = "gpt-3.5-turbo";
    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; } = new List<Message>();
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; } = 0.5f;
}