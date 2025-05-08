using Legba.Engine.Models;

namespace Legba.Engine.Services;

public interface ILlmConnector
{
    Task<LegbaResponse> AskAsync(LegbaRequest request);
}