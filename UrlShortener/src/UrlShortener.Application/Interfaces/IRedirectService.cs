using   UrlShortener.Application.DTOs;
namespace UrlShortener.Application.Interfaces;


public interface IRedirectService
{
    Task<string?> ResolveUrlAsync(string code);

    Task<string?> Login(LoginRequest request);
}
