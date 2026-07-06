using UrlShortener.Domain.Common;

namespace UrlShortener.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    private User()
    {
    }

    public User(string username,string email,string passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }
}