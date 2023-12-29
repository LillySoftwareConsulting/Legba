using System.Text.Json.Nodes;

namespace OpenAiConnector.KeyReaders;

public class JsonStringKeyReader : IApiKeyReader, IOrganizationIdReader
{
    private readonly string _openAiApiKey;

    public JsonStringKeyReader(string jsonText, string path)
    {
        JsonNode parsedJson =
            JsonNode.Parse(jsonText)
            ?? throw new InvalidOperationException($"Could not parse {nameof(jsonText)}");

        _openAiApiKey =
            parsedJson.ToString().GetValueFromJsonPath(path)
            ?? throw new InvalidOperationException($"Path '{path}' not found in {nameof(jsonText)}");
    }

    public string GetKey()
    {
        return _openAiApiKey;
    }
}