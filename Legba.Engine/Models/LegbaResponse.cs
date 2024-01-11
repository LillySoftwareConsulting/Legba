namespace Legba.Engine.Models;

public class LegbaResponse
{
    public string Text { get; set; } = string.Empty;
    public int RequestTokenCount { get; set; } = 0;
    public int ResponseTokenCount { get; set; } = 0;
}