using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Legba.Engine.Models.OpenAi;

public class OpenAiConnector(IHttpClientFactory httpClientFactory,
    Settings.Llm llm, Settings.Model model)
{
    #region Private fields

    private static readonly JsonSerializerOptions s_jsonSerializerOptions =
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

    public async Task<OpenAiResponse> AskAsync(OpenAiRequest request)
    {
        // Send request to the LLM API
        var response =
            await GetHttpClient()
            .PostAsJsonAsync(new Uri(Model.Url), request, s_jsonSerializerOptions)
            .ConfigureAwait(false);

        // Throw exception if the response was not successful
        response.EnsureSuccessStatusCode();

        // Parse the successful response
        var openAiResponse = 
            await response
                .Content.ReadFromJsonAsync<OpenAiResponse>(s_jsonSerializerOptions)
                ?? throw new Exception("Error parsing the API response");

        return openAiResponse;
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

        if (Llm.Name == Enums.Llm.Perplexity)
        {
            // Set the authorization header
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Llm.Keys.ApiKey);
        }

        return httpClient;
    }

    #endregion
}