using System.Text;

namespace Legba.Engine;

public static class ExtensionMethods
{
    public static bool IsNotNullEmptyOrWhitespace(this string? value) =>
        !string.IsNullOrWhiteSpace(value);

    public static bool IsNullEmptyOrWhitespace(this string? value) =>
        value == null || string.IsNullOrWhiteSpace(value);

    public static void AppendLineIfNotEmpty(this StringBuilder sb, string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            sb.AppendLine(text);
        }
    }
}