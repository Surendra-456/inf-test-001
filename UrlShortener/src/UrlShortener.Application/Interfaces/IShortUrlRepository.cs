using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Interfaces;

public interface IShortUrlRepository
{
    Task AddAsync(ShortUrl shortUrl,CancellationToken cancellationToken);

    Task<ShortUrl?> GetByCodeAsync(string urlCode,CancellationToken cancellationToken);

    Task<List<ShortUrl>> GetUserUrlsAsync(Guid userId,CancellationToken cancellationToken);
}