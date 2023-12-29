namespace OpenAiConnector.KeyReaders;

public class AzureKeyVaultKeyReader : IApiKeyReader, IOrganizationIdReader
{
    public string GetKey()
    {
        throw new NotImplementedException();
    }
}