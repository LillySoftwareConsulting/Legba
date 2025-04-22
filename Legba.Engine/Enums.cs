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
        Perplexity,
        [Description("Grok")]
        Grok,
        [Description("Groq")]
        Groq
    }

    public enum Role
    {
        Unspecified,
        User,
        Assistant
    }
}