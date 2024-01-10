namespace Legba.Engine.Models;

public class Settings
{
    public string llm { get; set; }
    public Keys keys { get; set; }

    public class Keys
    {
        public string apiKey { get; set; }
        public string orgId { get; set; }
    }
}