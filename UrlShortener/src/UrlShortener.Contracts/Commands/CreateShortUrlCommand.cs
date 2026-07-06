namespace UrlShortener.Contracts.Commands;

public record CreateShortUrlCommand(string OriginalUrl,Guid UserId);