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

    public async Task<LlmResponse> AskAsync(LlmRequest request)
    {
        try
        {
            HttpClient httpClient = GetHttpClient();

            Request openAiRequest = BuildRequest(request.Prompt);

            var response =
                await httpClient
                .PostAsJsonAsync(_uri, openAiRequest, _jsonSerializerOptions)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            Response openAiResponse = 
                await response
                    .Content.ReadFromJsonAsync<Response>(_jsonSerializerOptions)
                    ?? throw new Exception("Error parsing the OpenAI API response");

            return new LlmResponse() { Text = openAiResponse.Choices[0].Message?.Content };
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to parse response", ex);
        }
    }

    //public async Task<Response> CallOpenAiApiAsync(string prompt,
    //    List<Message>? priorMessages = null)
    //{
    //    try
    //    {
    //        HttpClient httpClient = GetHttpClient();

    //        Request request = BuildRequest(prompt, priorMessages);

    //        var response =
    //            await httpClient
    //            .PostAsJsonAsync(_uri, request, _jsonSerializerOptions)
    //            .ConfigureAwait(false);

    //        response.EnsureSuccessStatusCode();

    //        return await response
    //            .Content.ReadFromJsonAsync<Response>(_jsonSerializerOptions)
    //            ?? throw new Exception("Error parsing the OpenAI API response");
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Unable to parse response", ex);
    //    }
    //}

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

    private static Request BuildRequest(string prompt, List<Message>? priorMessages = null)
    {
        Request request = new();

        if (priorMessages is not null)
        {
            request.Messages.AddRange(priorMessages);
        }

        request.Messages.Add(new Message { Role = Enums.Role.User, Content = prompt });

        return request;
    }

    #endregion
}