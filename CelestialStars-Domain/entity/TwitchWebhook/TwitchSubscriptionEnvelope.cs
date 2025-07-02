using CelestialStars_Domain.entity.TwitchWebhook;
using CelestialStars_Domain.TwitchWebhook;

namespace CelestialStars_Application.webhooks.twitch;

public class TwitchSubscriptionEnvelope
{
    public Subscription Subscription { get; set; }
}