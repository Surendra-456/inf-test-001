using System.Net.Http.Json;
using UrlShortener.Forward.Mvc.Models;
namespace UrlShortener.Forward.Mvc.Services;


public class ForwardApiClient
{
    private readonly IHttpClientFactory  _httpClient;

    public ForwardApiClient(IHttpClientFactory  httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> GetDestinationUrl(string code)
    {
        var client =_httpClient.CreateClient("ForwarderApi");

        var result =await client.GetFromJsonAsync<ForwardResponse>($"api/forward/{code}");
        return result?.DestinationUrl;
    }
}

