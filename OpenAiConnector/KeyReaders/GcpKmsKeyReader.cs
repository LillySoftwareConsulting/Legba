namespace OpenAiConnector.KeyReaders;

public class GcpKmsKeyReader : IApiKeyReader, IOrganizationIdReader
{
    private readonly string _openAiApiKey;

    public string GetKey()
    {
        return _openAiApiKey;
    }
}