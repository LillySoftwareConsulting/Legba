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
        switch(llm.Name)
        {
            case Enums.Llm.OpenAi:
                return new OpenAi.OpenAiConnector(_httpClientFactory, llm, model);
            default:
                throw new Exception($"Unknown LLM Connector: {llm.Name} - {model.Name}");
        }
    }
}
