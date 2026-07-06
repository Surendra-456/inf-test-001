namespace UrlShortener.Application.Interfaces;

public interface IRedirectService
{
    Task<string?> ResolveAsync(string code);
}