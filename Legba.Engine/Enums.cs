using System.ComponentModel;

namespace Legba.Engine;

public class Enums
{
    public enum Llm
    {
        Unspecified,
        [Description("OpenAI")]
        OpenAi,
        [Description("Perplexity")]
        Perplexity
    }

    public enum Role
    {
        Unspecified,
        User,
        Assistant
    }
}