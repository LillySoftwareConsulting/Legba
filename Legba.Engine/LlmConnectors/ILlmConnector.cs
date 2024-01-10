using Legba.Engine.Models;

namespace Legba.Engine.LlmConnectors;

public interface ILlmConnector
{
    Response Ask(Request request);
}