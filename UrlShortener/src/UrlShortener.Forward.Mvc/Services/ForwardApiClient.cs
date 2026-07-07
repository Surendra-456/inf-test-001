using System.Net.Http.Json;
using UrlShortener.Forward.Mvc.Models;
using System.Net.Http.Headers;

namespace UrlShortener.Forward.Mvc.Services;

public class ForwardApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ForwardApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string?> GetDestinationUrl(string code)
    {
        var client = _httpClientFactory.CreateClient("ForwarderApi");

        var token = _httpContextAccessor.HttpContext?.Session.GetString("JWT_TOKEN");

        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", token);
        }

        var result = await client.GetFromJsonAsync<ForwardResponse>($"api/forward/{code}");

        return result?.DestinationUrl;
    }
    public async Task<string?> LoginAsync(LoginRequest request)
    {
        var client = _httpClientFactory.CreateClient("ForwarderApi");

        var response = await client.PostAsJsonAsync("api/forward/login",request);
        
    Console.WriteLine($"Login: {response}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        return result?.Token;
    }
}