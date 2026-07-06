using UrlShortener.Assigner.Worker;
using UrlShortener.Infrastructure;
using UrlShortener.Assigner.Worker.Actors;
using UrlShortener.Contracts;
using Akka.Hosting;
using Akka.Remote.Hosting;
using Akka.Cluster.Hosting;
using UrlShortener.Infrastructure.Persistence;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<ShortUrlManagerActor>();
builder.Services.AddScoped<UrlLookupActor>();
builder.Services.AddAkka(
    "UrlShortenerSystem",
    akka =>
    {
        akka
            .WithRemoting(
                "localhost",
                8110)
            .WithClustering(
                new ClusterOptions
                {
                    Roles = ["assigner"],
                    SeedNodes =
                    [
                        "akka.tcp://UrlShortenerSystem@localhost:8110"
                    ]
                });
    });

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var db =scope.ServiceProvider.GetRequiredService<UrlShortenerDbContext>();

    await DbSeeder.SeedAsync(db);
}

host.Run();
