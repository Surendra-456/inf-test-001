using UrlShortener.Assigner.Worker;
using UrlShortener.Infrastructure;
using UrlShortener.Assigner.Worker.Actors;
using UrlShortener.Contracts;
using Akka.Hosting;
using Akka.Remote.Hosting;
using Akka.Cluster.Hosting;
using UrlShortener.Infrastructure.Persistence;
using Akka.Actor;
using Akka.DependencyInjection;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<ShortUrlManagerActor>();
builder.Services.AddScoped<UrlLookupActor>();
// builder.Services.AddAkka(
//     "UrlShortenerSystem",
//     akka =>
//     {
//         akka
//             .WithRemoting(
//                 "localhost",
//                 8110)
//             .WithClustering(
//                 new ClusterOptions
//                 {
//                     Roles = ["assigner"],
//                     SeedNodes =
//                     [
//                         "akka.tcp://UrlShortenerSystem@localhost:8110"
//                     ]
//                 });
//     });
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

        akka.WithActors((system, registry) =>
        {
            var resolver =
                DependencyResolver.For(system);

            var props =resolver.Props<ShortUrlManagerActor>();

            var urlManager =
                system.ActorOf(
                    props,
                    ActorNames.UrlManager);
Console.WriteLine($"Actor created : {urlManager.Path}");

            registry.Register<ShortUrlManagerActor>(
                urlManager);
        });
    });

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var db =scope.ServiceProvider.GetRequiredService<UrlShortenerDbContext>();

    await DbSeeder.SeedAsync(db);
}

host.Run();
