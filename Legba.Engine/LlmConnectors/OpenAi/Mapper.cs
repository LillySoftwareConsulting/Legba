using Legba.Engine.Models;

namespace Legba.Engine.LlmConnectors.OpenAi;

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
            Text = source.Choices[0].Message?.Content ?? string.Empty,
            RequestTokenCount = source.Usage.PromptTokens,
            ResponseTokenCount = source.Usage.CompletionTokens
        };
    }
}