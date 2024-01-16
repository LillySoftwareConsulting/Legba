namespace Legba.Engine.Models;

public abstract class PromptPrefix
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}