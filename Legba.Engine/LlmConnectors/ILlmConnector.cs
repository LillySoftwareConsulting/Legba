using Legba.Engine.Models;

namespace Legba.Engine.LlmConnectors;

public interface ILlmConnector
{
    Task<LlmResponse> AskAsync(LlmRequest request);
}