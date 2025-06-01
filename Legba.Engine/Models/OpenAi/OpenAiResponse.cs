using System.Text.Json.Serialization;

namespace Legba.Engine.Models.OpenAi;

public class OpenAiResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("object")]
    public string _object { get; set; } = string.Empty;
    [JsonPropertyName("created")]
    public int Created { get; set; }
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;
    [JsonPropertyName("usage")]
    public Usage Usage { get; set; } = new Usage();
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; } = [];
}