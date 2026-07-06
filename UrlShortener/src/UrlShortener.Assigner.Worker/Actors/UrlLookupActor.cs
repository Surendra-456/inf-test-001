using Akka.Actor;
using UrlShortener.Application.Interfaces;
using UrlShortener.Contracts.Messages;

namespace UrlShortener.Assigner.Worker.Actors;

public class UrlLookupActor : ReceiveActor
{
    private readonly IShortUrlRepository _repository;

    public UrlLookupActor(IShortUrlRepository repository)
    {
        _repository = repository;

        ReceiveAsync<ResolveShortUrlMessage>(ResolveAsync);
    }

    private async Task ResolveAsync( ResolveShortUrlMessage message)
    {
        

        var result =await _repository.GetByCodeAsync(message.code,CancellationToken.None);

        Sender.Tell(new ResolvedShortUrlMessage(result?.OriginalUrl));
    }
}