using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Persistence;

namespace UrlShortener.Infrastructure.Repositories;

public class ShortUrlRepository : IShortUrlRepository
{
    private readonly UrlShortenerDbContext _dbContext;

    public ShortUrlRepository(UrlShortenerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ShortUrl shortUrl,CancellationToken cancellationToken)
    {
        _dbContext.ShortUrls.Add(shortUrl);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ShortUrl?> GetByCodeAsync(string urlCode,CancellationToken cancellationToken)
    {
        return await _dbContext.ShortUrls.FirstOrDefaultAsync(x => x.UrlCode == urlCode,cancellationToken);
    }

    public async Task<List<ShortUrl>> GetUserUrlsAsync(Guid userId,CancellationToken cancellationToken)
    {
        return await _dbContext.ShortUrls.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
    }
}