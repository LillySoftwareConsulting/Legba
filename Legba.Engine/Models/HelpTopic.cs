namespace Legba.Engine.Models;

public class HelpTopic(string title, string content)
{
    public string Title { get; set; } = title;
    public string Content { get; set; } = content;
}