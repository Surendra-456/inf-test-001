using UrlShortener.Domain.Common;

namespace UrlShortener.Domain.Entities;

public class ShortUrl : BaseEntity
{
    public string UrlCode { get; private set; } = string.Empty;

    public string OriginalUrl { get; private set; } = string.Empty;

    public Guid UserId { get; private set; }

    public DateTime CreatedUtc { get; private set; }

    private ShortUrl()
    {
    }

    public ShortUrl(string urlCode,string originalUrl,Guid userId)
    {
        UrlCode = urlCode;
        OriginalUrl = originalUrl;
        UserId = userId;
        CreatedUtc = DateTime.UtcNow;
    }
}