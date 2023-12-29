using Microsoft.Extensions.Configuration;

namespace KeyReader;

public class UserSecretsKeyReader : IApiKeyReader, IOrganizationIdReader
{
    private readonly string _openAiApiKey;

    public UserSecretsKeyReader(IConfigurationRoot configuration, string pathName)
    {
        _openAiApiKey = configuration[pathName]
            ?? throw new Exception($"Unable to find value at path '{pathName}'");
    }

    public string GetKey()
    {
        return _openAiApiKey;
    }
}