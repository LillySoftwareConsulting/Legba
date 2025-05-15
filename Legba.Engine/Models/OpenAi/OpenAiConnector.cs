using System.Text.Json;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Net.Http;

namespace Legba.Engine.Models.OpenAi;

public class OpenAiConnector(IHttpClientFactory httpClientFactory,
    Settings.Llm llm, Settings.Model model)
{
    #region Private fields

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

    #region Properties

    public Settings.Llm Llm { get; } = llm;
    public Settings.Model Model { get; } = model;

    #endregion

    public async Task<LegbaResponse> AskAsync(LegbaRequest legbaRequest)
    {
        try
        {
            var openAiRequest = MapToOpenAiRequest(legbaRequest);

            // Send the request to OpenAI
            var httpClient = GetHttpClient();

            if(Llm.Name == Enums.Llm.Perplexity)
            {
                // Set the authorization header
                httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", Llm.Keys.ApiKey);
            }

            var uri = new Uri(Model.Url);
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
                    ?? throw new Exception("Error parsing the API response");

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
        var httpClient = httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Llm.Keys.ApiKey}");

        if (Llm.Keys.OrgId.IsNotNullEmptyOrWhitespace())
        {
            httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", Llm.Keys.OrgId);
        }

        return httpClient;
    }

    private OpenAiRequest MapToOpenAiRequest(LegbaRequest legbaRequest)
    {
        return new OpenAiRequest()
        {
            Model = Model.Id,
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