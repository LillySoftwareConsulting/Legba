using Legba.Engine.Models;
using System.Collections.ObjectModel;
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

    public static Settings.Llm LlmWithModel(this ObservableCollection<Settings.Llm> llms, 
        Settings.Model model)
    {
        foreach (var llm in llms)
        {
            if (llm.Models.Contains(model))
            {
                return llm;
            }
        }

        throw new ArgumentException($"Model {model} not found in the list of LLMs.");
    }
}