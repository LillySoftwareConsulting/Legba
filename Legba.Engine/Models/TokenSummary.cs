namespace Legba.Engine.Models;

public class TokenSummary
{
    public int RequestTokenCount { get; set; } = 0;
    public int ResponseTokenCount { get; set; } = 0;
    public int TotalTokenCount => 
        RequestTokenCount + ResponseTokenCount;
}