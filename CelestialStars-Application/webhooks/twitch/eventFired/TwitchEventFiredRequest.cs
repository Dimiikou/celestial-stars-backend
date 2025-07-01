using CelestialStars_Domain.TwitchWebhook;
using MediatR;

namespace CelestialStars_Application.webhooks.twitch.eventFired;

public class TwitchEventFiredRequest : TwitchSubscriptionEnvelope, IRequest<Unit>
{
    public EventData Event { get; set; }
}