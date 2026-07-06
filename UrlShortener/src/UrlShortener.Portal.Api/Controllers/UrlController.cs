using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Portal.Api.Models;
using System.Security.Claims;
using UrlShortener.Contracts.Messages;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Akka.Actor;

[Authorize]
[ApiController]
[Route("api/url")]
public class UrlController : ControllerBase
{
    private readonly IShortUrlRepository _urlRepo;
    private readonly IActorRef _portalActor;
    public UrlController(IShortUrlRepository urlRepo,IActorRef portalActor)
    {
        _urlRepo = urlRepo;
        _portalActor=portalActor;
    }

    [HttpPost("short")]
    public async Task<IActionResult> AddShortUrl(CreateShortUrlRequest request)
    {        
      
     var userId = Guid.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value!);
     
     var result = await _portalActor.Ask<ShortUrlCreatedMessage>(new CreateShortUrlMessage(request.OriginalUrl, userId));
       return Ok(result);

    }

    
}