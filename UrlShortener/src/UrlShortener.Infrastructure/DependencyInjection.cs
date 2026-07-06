using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Interfaces;
using UrlShortener.Infrastructure.Persistence;
using UrlShortener.Infrastructure.Repositories;

namespace UrlShortener.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,string connectionString)
    {
        services.AddDbContext<UrlShortenerDbContext>(
            options =>
                options.UseSqlite(connectionString));

        services.AddScoped<IShortUrlRepository,ShortUrlRepository>();
        services.AddScoped<IUserRepository,UserRepository>();
        return services;
    }
}