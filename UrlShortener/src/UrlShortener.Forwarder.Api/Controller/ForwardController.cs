using Microsoft.AspNetCore.Mvc;
using UrlShortener.Forwarder.Api.Services;
using  UrlShortener.Application.Interfaces;
using  UrlShortener.Infrastructure.Repositories;
using   UrlShortener.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace UrlShortener.Forwarder.Api.Controllers;

[ApiController]
[Route("api/forward")]
public class ForwardController : ControllerBase
{
    private readonly IRedirectService _service;

    public ForwardController(IRedirectService service)
    {
        _service = service;
    }
    [Authorize]
    [HttpGet("{code}")]
    public async Task<IActionResult> RedirectResolve(string code)
    {
        var url = await _service.ResolveUrlAsync(code);
        if (url is null)
        {
            return NotFound();
        }

        return Ok(new {destinationUrl = url});
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var token =await _service.Login(request);

        if (token is null)
            return Unauthorized();


        return Ok(new {token});
    }
}