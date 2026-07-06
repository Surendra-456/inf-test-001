using Akka.Actor;
using UrlShortener.Contracts.Messages;

namespace UrlShortener.Forwarder.Api.Actors;

public class RedirectActor : ReceiveActor
{
    public RedirectActor(
        IActorRef lookupActor)
    {
        ReceiveAsync<ResolveShortUrlMessage>(
            async msg =>
            {
                var result =await lookupActor.Ask<ResolvedShortUrlMessage>(msg);
                Sender.Tell(result);
            });
    }
}