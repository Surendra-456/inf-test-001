using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Persistence;

public class UrlShortenerDbContext : DbContext
{
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options): base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ShortUrl>()
            .HasIndex(x => x.UrlCode)
            .IsUnique();

        builder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();
    }
}