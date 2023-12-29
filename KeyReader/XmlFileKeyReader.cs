using System.Xml;

namespace KeyReader;

public class XmlFileKeyReader : IApiKeyReader, IOrganizationIdReader
{
    private readonly string _openAiApiKey;

    public XmlFileKeyReader(string fileName, string xPath)
    {
        try
        {
            // Load the XML document
            XmlDocument xmlDoc = new();
            xmlDoc.Load(fileName);

            // Select the node using XPath
            var node = xmlDoc.SelectSingleNode(xPath);

            if (node == null)
            {
                throw new XmlException($"XPath {xPath} does not exist in {fileName}.");
            }

            // Get the node's value
            string apiKey = node.InnerText;

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new XmlException($"XPath {xPath} does not have a value.");
            }

            _openAiApiKey = apiKey;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException("File does not exist.");
        }
        catch (XmlException ex)
        {
            throw new XmlException("XML parsing error: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred: " + ex.Message);
        }
    }

    public string GetKey()
    {
        return _openAiApiKey;
    }
}