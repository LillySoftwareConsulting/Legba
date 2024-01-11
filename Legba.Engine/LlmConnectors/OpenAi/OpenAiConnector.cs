using System.Text.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Legba.Engine.Models;

namespace Legba.Engine.LlmConnectors.OpenAi;

public class OpenAiConnector : ILlmConnector
{
    #region Properties and backing fields

    private readonly Uri _uri;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _openAiApiKey = string.Empty;
    private readonly string? _openAiOrganizationId;

    private readonly JsonSerializerOptions _jsonSerializerOptions =
        new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

    #endregion

    public OpenAiConnector(Settings settings, IHttpClientFactory httpClientFactory)
    {
        _uri = new Uri("https://api.openai.com/v1/chat/completions");
        _httpClientFactory = httpClientFactory;
        _openAiApiKey = settings.keys.apiKey;
        _openAiOrganizationId = settings.keys.orgId;
    }

    public async Task<LegbaResponse> AskAsync(LegbaRequest legbaRequest)
    {
        try
        {
            var openAiRequest = MapToOpenAiRequest(legbaRequest);

            // Send the request to OpenAI
            var httpClient = GetHttpClient();

            var response =
                await httpClient
                .PostAsJsonAsync(_uri, openAiRequest, _jsonSerializerOptions)
                .ConfigureAwait(false);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();

            // Parse the successful response
            var openAiResponse = 
                await response
                    .Content.ReadFromJsonAsync<OpenAiResponse>(_jsonSerializerOptions)
                    ?? throw new Exception("Error parsing the OpenAI API response");

            var legbaResponse = MapToLegbaResponse(openAiResponse);

            return legbaResponse;
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to parse response", ex);
        }
    }

    #region Private methods

    private HttpClient GetHttpClient()
    {
        var httpClient = _httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAiApiKey}");

        if (_openAiOrganizationId.IsNotNullEmptyOrWhitespace())
        {
            httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", _openAiOrganizationId);
        }

        return httpClient;
    }

    private static OpenAiRequest MapToOpenAiRequest(LegbaRequest legbaRequest)
    {
        return new OpenAiRequest()
        {
            Model = string.IsNullOrWhiteSpace(legbaRequest.Model) 
                ? "gpt-3.5-turbo" 
                : legbaRequest.Model,
            Messages = legbaRequest.Messages,
            Temperature = legbaRequest.Temperature
        };
    }

    private static LegbaResponse MapToLegbaResponse(OpenAiResponse openAiResponse)
    {
        return new LegbaResponse()
        {
            Text = openAiResponse.Choices[0].Message?.Content ?? string.Empty,
            RequestTokenCount = openAiResponse.Usage.PromptTokens,
            ResponseTokenCount = openAiResponse.Usage.CompletionTokens
        };
    }

    #endregion
}