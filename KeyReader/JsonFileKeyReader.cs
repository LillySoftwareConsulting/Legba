﻿using System.Text.Json.Nodes;

namespace KeyReader;

public class JsonFileKeyReader : IApiKeyReader, IOrganizationIdReader
{
    private readonly string _openAiApiKey;

    public JsonFileKeyReader(string fileName, string path)
    {
        try
        {
            string fileContent =
                File.ReadAllText(fileName);

            JsonNode parsedJson =
                JsonNode.Parse(fileContent)
                ?? throw new InvalidOperationException($"Could not parse {fileName}.");

            _openAiApiKey =
                parsedJson.ToString().GetValueFromJsonPath(path)
                ?? throw new InvalidOperationException($"Path '{path}' not found in {fileName}.");
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"File {fileName} does not exist.");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error reading JSON file: {ex.Message}", ex);
        }
    }

    public string GetKey()
    {
        return _openAiApiKey;
    }
}