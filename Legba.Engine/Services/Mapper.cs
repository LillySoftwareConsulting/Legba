using Legba.Engine.Models;
using Legba.Engine.LlmConnectors.OpenAi;

namespace Legba.Engine.Services;

public static class Mapper
{
    public static OpenAiRequest Map(LegbaRequest source)
    {
        return new OpenAiRequest()
        {
            Model = string.IsNullOrWhiteSpace(source.Model) ? "gpt-3.5-turbo" : source.Model,
            Messages = source.Messages,
            Temperature = source.Temperature
        };
    }

    public static LegbaResponse Map(OpenAiResponse source)
    {
        return new LegbaResponse()
        {
            Text = source.Choices[0].Message?.Content
        };
    }
}