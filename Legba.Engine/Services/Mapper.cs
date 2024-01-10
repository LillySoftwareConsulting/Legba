using Legba.Engine.Models;
using Legba.Engine.LlmConnectors.OpenAi;

namespace Legba.Engine.Services;

public static class Mapper
{
    public static Request Map<T1, T2>(this T1 source) 
        where T1 : LegbaRequest 
        where T2 : Request, new()
    {
        return new Request()
        {
            Model = string.IsNullOrWhiteSpace(source.Model) ? "gpt-3.5-turbo" : source.Model,
            Messages = source.Messages,
            Temperature = source.Temperature
        };
    }
}