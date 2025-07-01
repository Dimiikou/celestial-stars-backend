using MediatR;

namespace CelestialStars_Application.webhooks.twitch.challengeRequest;

public class TwitchChallengeRequest : TwitchSubscriptionEnvelope, IRequest<Unit>
{
    public string Challenge { get; set; }
}