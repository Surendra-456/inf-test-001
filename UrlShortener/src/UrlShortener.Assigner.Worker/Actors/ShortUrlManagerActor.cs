using Akka.Actor;
using UrlShortener.Application.Interfaces;
using UrlShortener.Contracts.Messages;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Assigner.Worker.Actors;

public class ShortUrlManagerActor : ReceiveActor
{
    private readonly IShortUrlRepository _repository;

    public ShortUrlManagerActor(IShortUrlRepository repository)
    {
        _repository = repository;
        ReceiveAsync<CreateShortUrlMessage>(CreateShortUrlAsync);
    }

    private async Task CreateShortUrlAsync(CreateShortUrlMessage message)
    {
        var code = GenerateCode();

        var shortUrl =$"http://localhost:5002/s/{code}";

        await _repository.AddAsync(
            new ShortUrl(
                code,
                message.OriginalUrl,
                message.UserId),
            CancellationToken.None);

        Sender.Tell(new ShortUrlCreatedMessage(shortUrl));
    }

    private static string GenerateCode()
    {
        return Guid.NewGuid().ToString("N")[..6].ToUpper();
    }
}