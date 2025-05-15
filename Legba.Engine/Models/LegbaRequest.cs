using Legba.Engine.Models.OpenAi;

namespace Legba.Engine.Models;

public class LegbaRequest
{
    public string Model { get; set; } = string.Empty;
    public List<Message> Messages { get; set; } = [];
    public float Temperature { get; set; } = 0.5f;
}