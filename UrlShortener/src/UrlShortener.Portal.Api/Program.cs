using Akka.Cluster.Hosting;
using Akka.Hosting;
using Akka.Remote.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UrlShortener.Application.Interfaces;
using UrlShortener.Infrastructure.Persistence;
using UrlShortener.Infrastructure.Repositories;
using UrlShortener.Infrastructure.Services;
using UrlShortener.Infrastructure;
using UrlShortener.Portal.Api.Actors;
using Akka.Actor;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
//     options.UseSqlServer(
//         builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddAkka(
    "UrlShortenerSystem",
    akka =>
    {
        akka
            .WithRemoting("localhost", 8111)
            .WithClustering(
                new ClusterOptions
                {
                    Roles = ["portal"],
                    SeedNodes =
                    [
                        "akka.tcp://UrlShortenerSystem@localhost:8110"
                    ]
                });

        akka.WithActors((system, registry) =>
        {
            var actor =
                system.ActorOf(
                    Props.Create(() =>
                        new PortalActor(ActorRefs.Nobody)),
                    "portal");

            registry.Register<PortalActor>(
                actor);
        });
    });

// builder.Services.AddAkka(
//     "UrlShortenerSystem",
//     akka =>
//     {
//         akka
//             .WithRemoting("localhost", 8111)
//             .WithClustering(
//                 new ClusterOptions
//                 {
//                     Roles = ["portal"],
//                     SeedNodes =
//                     [
//                         "akka.tcp://UrlShortenerSystem@localhost:8110"
//                     ]
//                 });
//     });

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            "SuperSecretJwtKey12345678901234567890"))
            };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Angular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();