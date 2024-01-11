using Legba.Engine.LlmConnectors.OpenAi;

namespace Legba.Engine.Models;

public class LegbaRequest
{
    public string Model { get; set; } = string.Empty;
    public List<Message> Messages { get; set; } = new List<Message>();
    public float Temperature { get; set; } = 0.5f;
}