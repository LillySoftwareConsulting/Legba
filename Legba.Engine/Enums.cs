using System.ComponentModel;

namespace Legba.Engine;

public class Enums
{
    public enum LlmConnectorType
    {
        Unspecified,
        [Description("OpenAI")]
        OpenAi
    }

    public enum Role
    {
        Unspecified,
        User,
        Assistant
    }
}