using Akka.Actor;
using UrlShortener.Contracts;
using UrlShortener.Contracts.Messages;

namespace UrlShortener.Portal.Api.Actors;

public class PortalActor : ReceiveActor
{
    private readonly ActorSelection _urlManager;

    public PortalActor()
    {
        _urlManager = Context.ActorSelection(
            $"akka.tcp://UrlShortenerSystem@localhost:8110/user/{ActorNames.UrlManager}");
        
        ReceiveAsync<CreateShortUrlMessage>(async message =>
        {
            var result =
                await _urlManager.Ask<ShortUrlCreatedMessage>(message);

            Sender.Tell(result);
        });
    }
}