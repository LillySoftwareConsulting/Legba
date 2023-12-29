using System.Text.Json.Serialization;

namespace OpenAiConnector.Models;

public class Choice
{
    [JsonPropertyName("message")]
    public Message? Message { get; set; }
    [JsonPropertyName("logprobs")]
    public object LogProbs { get; set; } = string.Empty;
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; } = string.Empty;
    [JsonPropertyName("index")]
    public int Index { get; set; }
}