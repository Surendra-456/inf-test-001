using Akka.Actor;
using UrlShortener.Contracts.Messages;
using  UrlShortener.Application.Interfaces;


namespace UrlShortener.Forwarder.Api.Services;

public class RedirectService : IRedirectService
{
    private readonly IActorRef _redirectActor;

    public RedirectService(IActorRef redirectActor)
    {
        _redirectActor = redirectActor;
    }

    public async Task<string?> ResolveAsync(string code)
    {
        var response =await _redirectActor.Ask<ResolvedShortUrlMessage>(new ResolveShortUrlMessage(code));

        return response.OriginalUrl;
    }
}