using System.Text.Json.Serialization;

namespace Legba.Engine.Models;

public class Settings
{
    [JsonPropertyName("llms")]
    public List<Llm> Llms { get; set; } = new List<Llm>();

    public class Llm
    {
        [JsonPropertyName("name")]
        public Enums.Llm Name { get; set; } = Enums.Llm.Unspecified;
        [JsonPropertyName("keys")]
        public Keys Keys { get; set; } = new Keys();
        [JsonPropertyName("models")]
        public List<Model> Models { get; set; } = new List<Model>();
    }

    public class Keys
    {
        [JsonPropertyName("apiKey")]
        public string ApiKey { get; set; } = string.Empty;
        [JsonPropertyName("orgId")]
        public string OrgId { get; set; } = string.Empty;
    }

    public class Model
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}