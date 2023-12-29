namespace KeyReader;

public class EnvironmentVariableKeyReader : IApiKeyReader, IOrganizationIdReader
{
    private readonly string _openAiApiKey;

    public EnvironmentVariableKeyReader(
        string environmentVariableName = "OPENAI_API_KEY")
    {
        string? variableValue =
            Environment.GetEnvironmentVariable(environmentVariableName);

        if (variableValue == null)
        {
            throw new ArgumentException(
                $"{environmentVariableName} environment variable not found.");
        }

        _openAiApiKey = variableValue;
    }

    public string GetKey()
    {
        return _openAiApiKey;
    }
}