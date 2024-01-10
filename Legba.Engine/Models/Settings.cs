using static Legba.Engine.Enums;

namespace Legba.Engine.Models;

public class Settings
{
    public Llm llm { get; set; } = Llm.Unspecified;
    public Keys keys { get; set; } = new Keys();

    public class Keys
    {
        public string apiKey { get; set; } = string.Empty;
        public string orgId { get; set; } = string.Empty;
    }
}