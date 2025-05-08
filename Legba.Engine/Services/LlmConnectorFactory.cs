using Legba.Engine.Models;
using Legba.Engine.Models.OpenAi;

namespace Legba.Engine.Services;

public class LlmConnectorFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LlmConnectorFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public OpenAiConnector GetLlmConnector(Settings.Llm llm, Settings.Model model)
    {
        return new OpenAiConnector(_httpClientFactory, llm, model);
    }
}
