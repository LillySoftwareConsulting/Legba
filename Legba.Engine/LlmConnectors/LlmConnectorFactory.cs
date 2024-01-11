using Legba.Engine.Models;

namespace Legba.Engine.LlmConnectors;

public class LlmConnectorFactory
{
    private readonly Settings _settings;
    private readonly IHttpClientFactory _httpClientFactory;

    public LlmConnectorFactory(Settings settings, IHttpClientFactory httpClientFactory)
    {
        _settings = settings;
        _httpClientFactory = httpClientFactory;
    }

    public ILlmConnector GetLlmConnector()
    {
        switch(_settings.llm)
        {
            case Enums.Llm.OpenAi:
                return new OpenAi.OpenAiConnector(_settings, _httpClientFactory);
            default:
                throw new Exception($"Unknown LLM Connector: {_settings.llm}");
        }
    }
}
