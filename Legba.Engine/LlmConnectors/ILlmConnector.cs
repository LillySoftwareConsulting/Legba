using Legba.Engine.Models;

namespace Legba.Engine.LlmConnectors;

public interface ILlmConnector
{
    Task<LegbaResponse> AskAsync(LegbaRequest request);
}