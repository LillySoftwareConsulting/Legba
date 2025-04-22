using Legba.Engine.Models;

namespace Legba.Engine.LlmConnectors;

public class LlmConnectorFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LlmConnectorFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public ILlmConnector GetLlmConnector(Settings.Llm llm, Settings.Model model)
    {
        return new OpenAi.OpenAiConnector(_httpClientFactory, llm, model);
    }
}
