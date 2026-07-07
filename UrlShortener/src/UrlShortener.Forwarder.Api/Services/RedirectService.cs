using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;
using   UrlShortener.Application.DTOs;
using Microsoft.AspNetCore.Authorization;



namespace UrlShortener.Forwarder.Api.Services;

public class RedirectService : IRedirectService
{
    private readonly IUserRepository _userRepo;
    private readonly IShortUrlRepository _urlRepo;
    private readonly IJwtTokenService _jwt;
    private readonly ILogger<RedirectService> _logger;

    public RedirectService(IUserRepository userRepo,IShortUrlRepository urlRepo,IJwtTokenService jwt,ILogger<RedirectService> logger)
    {
        _userRepo = userRepo;
        _urlRepo = urlRepo;
        _jwt = jwt;
        _logger = logger;
    }

    public async Task<string?> ResolveUrlAsync(string code)
    {
        try
        {
            var url = await _urlRepo.GetByCodeAsync(code,CancellationToken.None);
           _logger.LogInformation("Code");
            return url?.OriginalUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resolving URL for code {Code}", code);
            throw;
        }
    }

    public async Task<string?> Login(LoginRequest request)
    {
        try
        {
            var user = await _userRepo.GetByEmailAsync(request.Email,CancellationToken.None);
           _logger.LogInformation("User logged in successfully for email {Email}",request.Email);
                if (user == null)
            {
                return null;
            }

            var isValidPassword =BCrypt.Net.BCrypt.Verify(request.Password,user.PasswordHash);
         
            if (!isValidPassword)
            {
          _logger.LogError("Invalid Credential");

                return null;
            }
          _logger.LogInformation("Valid Crdential");

            return _jwt.GenerateToken( user.Id, user.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error during login for email {Email}",request.Email);
            throw;
        }
    }
}