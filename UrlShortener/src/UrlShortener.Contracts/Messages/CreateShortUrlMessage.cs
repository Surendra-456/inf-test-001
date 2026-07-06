namespace UrlShortener.Contracts.Messages;

public record CreateShortUrlMessage(string OriginalUrl,Guid UserId);