using System.Net.Http.Json;
using System.Net.Http.Headers;
using UrlShortener.Forward.Mvc.Models;

namespace UrlShortener.Forward.Mvc.Services;

public class ForwardApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ForwardApiClient> _logger;

    public ForwardApiClient(IHttpClientFactory httpClientFactory,IHttpContextAccessor httpContextAccessor,ILogger<ForwardApiClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<string?> GetDestinationUrl(string code)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ForwarderApi");

            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWT_TOKEN");


            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", token);

                _logger.LogInformation("Authorization header added for code {Code}",code);
            }

            var result = await client.GetFromJsonAsync<ForwardResponse>($"api/forward/{code}");

            _logger.LogInformation("Destination URL resolved for code {Code}",code);

            return result?.DestinationUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error while resolving destination URL for code {Code}",code);
            throw;
        }
    }

    public async Task<string?> LoginAsync(LoginRequest request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ForwarderApi");

            _logger.LogInformation("Attempting login for email {Email}",request.Email);

            var response = await client.PostAsJsonAsync("api/forward/login",request);

            _logger.LogInformation("Login API responded with status code {StatusCode} for email {Email}",response.StatusCode,request.Email);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Login failed for email {Email}", request.Email);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _logger.LogInformation("Login successful for email {Email}",request.Email);

            return result?.Token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error during login for email {Email}",request.Email);
            throw;
        }
    }
}