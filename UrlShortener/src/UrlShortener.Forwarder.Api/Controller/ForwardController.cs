using Microsoft.AspNetCore.Mvc;
using UrlShortener.Forwarder.Api.Services;
using  UrlShortener.Application.Interfaces;


namespace UrlShortener.Forwarder.Api.Controllers;

[ApiController]
[Route("api/forward")]
public class ForwardController : ControllerBase
{
    private readonly IRedirectService _service;

    public ForwardController(
        IRedirectService service)
    {
        _service = service;
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> Resolve(string code)
    {
        var url = await _service.ResolveAsync(code);
        if (url is null)
        {
            return NotFound();
        }

        return Ok(new {destinationUrl = url});
    }
}