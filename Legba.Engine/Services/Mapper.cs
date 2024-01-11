using Legba.Engine.Models;
using Legba.Engine.LlmConnectors.OpenAi;

namespace Legba.Engine.Services;

public static class Mapper
{
    public static T MapTo<T>(this LegbaRequest source) where T : OpenAiRequest, new()
    {
        return new T()
        {
            Model = string.IsNullOrWhiteSpace(source.Model) ? "gpt-3.5-turbo" : source.Model,
            Messages = source.Messages,
            Temperature = source.Temperature
        };
    }

    public static T MapTo<T>(this OpenAiResponse source) where T : LegbaResponse, new()
    {
        return new T()
        {
            Text = source.Choices[0].Message?.Content ?? string.Empty
        };
    }
}