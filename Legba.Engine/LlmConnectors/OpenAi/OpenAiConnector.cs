using System.Text.Json;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Legba.Engine.Models;

namespace Legba.Engine.LlmConnectors.OpenAi;

public class OpenAiConnector : ILlmConnector
{
    #region Properties and backing fields

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Settings.Llm _llm;
    private readonly Settings.Model _model;

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

    public OpenAiConnector(IHttpClientFactory httpClientFactory, 
        Settings.Llm llm, Settings.Model model)
    {
        _llm = llm;
        _model = model;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<LegbaResponse> AskAsync(LegbaRequest legbaRequest)
    {
        try
        {
            var openAiRequest = MapToOpenAiRequest(legbaRequest);

            // Send the request to OpenAI
            var httpClient = GetHttpClient();

            if(_llm.Name == Enums.Llm.Perplexity)
            {
                // Set the authorization header
                httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", _llm.Keys.ApiKey);
            }

            var uri = new Uri(_model.Url);
            var response =
                await httpClient
                .PostAsJsonAsync(uri, openAiRequest, _jsonSerializerOptions)
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
            return new LegbaResponse()
            {
                Text = $"Error: {ex.Message}"
            };
        }
    }

    #region Private methods

    private HttpClient GetHttpClient()
    {
        var httpClient = _httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_llm.Keys.ApiKey}");

        if (_llm.Keys.OrgId.IsNotNullEmptyOrWhitespace())
        {
            httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", _llm.Keys.OrgId);
        }

        return httpClient;
    }

    private OpenAiRequest MapToOpenAiRequest(LegbaRequest legbaRequest)
    {
        return new OpenAiRequest()
        {
            Model = _model.Id,
            Messages = legbaRequest.Messages,
            Temperature = legbaRequest.Temperature
        };
    }

    private LegbaResponse MapToLegbaResponse(OpenAiResponse openAiResponse)
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