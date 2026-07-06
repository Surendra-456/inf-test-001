using Akka.Actor;
using UrlShortener.Contracts.Messages;

namespace UrlShortener.Portal.Api.Actors;

public class PortalActor : ReceiveActor
{
    public PortalActor(IActorRef urlManager)
    {
        ReceiveAsync<CreateShortUrlMessage>(
            async message =>
            {
                var result =await urlManager.Ask<ShortUrlCreatedMessage>(message);

                Sender.Tell(result);
            });
    }
}