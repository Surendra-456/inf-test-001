using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(
        UrlShortenerDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Users.AnyAsync())
        {
            var user = new User(
                "admin",
                "admin@gmail.com",
                BCrypt.Net.BCrypt.HashPassword("Admin@123"));

            context.Users.Add(user);

            await context.SaveChangesAsync();
        }
    }
}