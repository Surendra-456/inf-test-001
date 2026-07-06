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
using UrlShortener.Contracts;

[Authorize]
[ApiController]
[Route("api/url")]
public class UrlController : ControllerBase
{
    private readonly IShortUrlRepository _urlRepo;
    private readonly ActorSystem _actorSystem;
    public UrlController(IShortUrlRepository urlRepo,ActorSystem actorSystem)
    {
        _urlRepo = urlRepo;
        _actorSystem=actorSystem;
    }

    [HttpPost("short")]
    public async Task<IActionResult> AddShortUrl(CreateShortUrlRequest request)
    {        
      
     var portalActor =_actorSystem.ActorSelection($"/user/{ActorNames.Portal}");
     var userId = Guid.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value!);
     
     var result = await portalActor.Ask<ShortUrlCreatedMessage>(new CreateShortUrlMessage(request.OriginalUrl, userId));
       return Ok(result);

    }

    
}