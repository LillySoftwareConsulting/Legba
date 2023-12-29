using System.Text.Json.Serialization;

namespace OpenAiConnector.Models;

public class Response
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("object")]
    public string _object { get; set; }
    [JsonPropertyName("created")]
    public int Created { get; set; }
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; } = new List<Choice>();
}