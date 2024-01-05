using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ArgosOnDemand.HttpServices;

public class OpenAIHttpService : IOpenAIProxy
{
    private readonly HttpClient _httpClient;
    private readonly string _subscriptionId;
    private readonly string _apiKey;

    public OpenAIHttpService(IConfiguration configuration)
    {
        var openApiUrl = configuration["OpenAi:Url"] ?? throw new ArgumentException(nameof(configuration));
        _httpClient = new HttpClient { BaseAddress = new Uri(openApiUrl) };

        _subscriptionId = configuration["OpenAi:OrganizationId"];
        _apiKey = configuration["OpenAi:ApiKey"];
    }

    public async Task<GenerateImageResponse> GenerateImages(GenerateImageRequest prompt, CancellationToken cancellation = default)
    {
        using var rq = new HttpRequestMessage(HttpMethod.Post, "/v1/images/generations");

        var jsonRequest = JsonSerializer.Serialize(prompt, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        rq.Content = new StringContent(jsonRequest);
        rq.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var apiKey = "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";
        rq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var subscriptionId = _subscriptionId;
        rq.Headers.TryAddWithoutValidation("OpenAI-Organization", subscriptionId);

        var response = await _httpClient.SendAsync(rq);

        response.EnsureSuccessStatusCode();

        var content = response.Content;

        var jsonResponse = await content.ReadFromJsonAsync<GenerateImageResponse>(cancellationToken: cancellation);

        return jsonResponse;
    }

    public async Task<byte[]> DownloadImage(string url)
    {
        var buffer = await _httpClient.GetByteArrayAsync(url);

        return buffer;
    }
}
