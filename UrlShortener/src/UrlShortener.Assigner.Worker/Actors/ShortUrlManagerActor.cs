using Akka.Actor;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Interfaces;
using UrlShortener.Contracts.Messages;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Assigner.Worker.Actors;

public class ShortUrlManagerActor : ReceiveActor
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ShortUrlManagerActor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        ReceiveAsync<CreateShortUrlMessage>(CreateShortUrlAsync);
    }

    private async Task CreateShortUrlAsync(CreateShortUrlMessage message)
    {
        using var scope = _scopeFactory.CreateScope();

        var repository =
            scope.ServiceProvider.GetRequiredService<IShortUrlRepository>();

        var code = GenerateCode();

        var shortUrl = $"http://yts/{code}";

        await repository.AddAsync(
            new ShortUrl(
                code,
                message.OriginalUrl,
                message.UserId),
            CancellationToken.None);

        Sender.Tell(
            new ShortUrlCreatedMessage(shortUrl));
    }

    private static string GenerateCode()
    {
        return Guid.NewGuid().ToString("N")[..6].ToUpper();
    }
}