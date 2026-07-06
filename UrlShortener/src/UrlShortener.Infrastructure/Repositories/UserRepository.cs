using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Persistence;

namespace UrlShortener.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UrlShortenerDbContext _db;

    public UserRepository(UrlShortenerDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetByEmailAsync(string email,CancellationToken ct)
    {
        return await _db.Users.FirstOrDefaultAsync(x => x.Email == email,ct);
    }

    public async Task AddAsync(User user,CancellationToken ct)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
    }
}